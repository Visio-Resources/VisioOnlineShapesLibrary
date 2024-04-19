import win32com.client
import pythoncom
import os
import time
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
from pathlib import Path
from models import *
from sqlalchemy import create_engine
from sqlalchemy_utils import database_exists
from sqlalchemy.orm import Session
from sqlalchemy import func

watchPath = '.\\upload'
imagePath = os.path.abspath("static\\images\\")
stencilPath = os.path.abspath("stencils\\")

visTypeStencil = 2
visSectionObject = 1
visRowMisc = 17
visObjKeywords = 27
visIconFormatVisio = 0
visIconFormatBMP = 2
visOpenRO = 2

class Handler(FileSystemEventHandler):
    @staticmethod
    def on_modified(event):
        filePath = Path(event.src_path)
        if not filePath.is_file():
            return
        
        suffix = filePath.suffix
        if not (suffix == ".vss" or suffix == ".vssx" or suffix == ".vssm"):
            return
        
        filePath = os.path.abspath(filePath)

        pythoncom.CoInitialize()
        vApp = win32com.client.GetObject(Class="Visio.Application")
        vDoc = vApp.Documents.OpenEx(filePath, visOpenRO)
        
        engine = create_engine('sqlite:///db.db', echo = False)

        if not database_exists(engine.url):
            Base.metadata.create_all(engine)

        with Session(engine) as session:
            masterId = session.query(func.count(Master.id)).scalar()
            stencilId = session.query(func.count(Stencil.id)).scalar()
            listMasters = []

            for mst in vDoc.Masters:
                masterId += 1
                listMasters.append(Master(
                    id = masterId,
                    name = mst.Name,
                    prompt = mst.Prompt,
                    keywords = mst.PageSheet.CellsSRC(visSectionObject, visRowMisc, visObjKeywords).ResultStr(""),
                    dataObject = ""
                ))
                mst.Export(f"{imagePath}\\{masterId}.png")

            stencilId += 1
            newStencil = Stencil(
                id = stencilId,
                fileName = vDoc.Name,
                title = vDoc.Title,
                subject = vDoc.Subject,
                author = vDoc.Creator,
                manager = vDoc.Manager,
                company = vDoc.Company,
                language = vDoc.Language,
                categories = vDoc.Category,
                tags = vDoc.Keywords,
                comments = vDoc.Description,
                masters = listMasters
            )
        
        session.add(newStencil)
        session.commit()
        vDoc.Close()
        os.rename(filePath, f"{stencilPath}\\{stencilId}{suffix}")

if __name__ == "__main__":
    event_handler = Handler()
    observer = Observer()
    observer.schedule(event_handler, watchPath, recursive=False)
    observer.start()

    try:
        while True:
            time.sleep(1)

    finally:
        observer.stop()
        observer.join()
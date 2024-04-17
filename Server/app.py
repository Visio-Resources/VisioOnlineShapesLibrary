from flask import Flask, render_template, request, send_file
from sqlalchemy import create_engine
from sqlalchemy import select, update
from sqlalchemy.orm import Session
from models import *
import os

app = Flask(__name__)

engine = create_engine('sqlite:///db.db', echo = True)
connect = engine.connect()
session = Session(connect)

@app.route("/", methods=['GET','POST'])
def search_masters():
    if request.method == 'POST':
        search = request.form['search']
    else:
        search = ""
    
    masters = session.scalars(select(Master).where(Master.name.contains(search)).order_by(Master.id)).all()
    return render_template('index.html', masters=masters)

@app.route('/download/<stencilId>')
def download(stencilId):
    stencil = session.get(Stencil, stencilId)
    filePath = os.path.abspath('stencils/' + stencilId + '.' + stencil.fileName.split('.')[-1])
    stencil.downloadCounter +=1
    session.commit()
    return send_file(filePath, download_name=stencil.fileName, as_attachment=True)

@app.route('/getshape/<masterId>')
def getshape(masterId):
    master = session.get(Master, masterId)
    return master.dataObject

if __name__ == "__main__":
    app.run(debug=True)
from flask import Flask, render_template, request, send_file
from sqlalchemy import create_engine
from sqlalchemy import select
from sqlalchemy.orm import Session
from sqlalchemy_utils import database_exists
from models import *
import os

app = Flask(__name__)
app.config['PREVIEW_FOLDER'] = os.path.abspath('./static/images')

engine = create_engine('sqlite:///db.db', echo = False)

if not database_exists(engine.url):
    Base.metadata.create_all(engine)

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
    master.downloadCounter += 1
    session.commit()
    return master.dataObject

@app.route("/addShape", methods=['GET','POST'])
def addShape():
    if request.method != 'POST':
        return ''
    if 'image' not in request.files:
        return ''
    file = request.files['image']
    if file.filename == '':
        return ''
    if not file:
        return ''
    if file.filename.rsplit('.', 1)[1].lower() != "png":
        return ''
    
    name = request.form['name']
    prompt = request.form['prompt']
    keywords = request.form['keywords']
    dataObject = request.form['dataObject']

    newId = session.query(func.count(Master.id)).scalar() + 1
    newShape = Master(
                id = newId,
                name = name,
                prompt = prompt,
                keywords = keywords,
                dataObject = dataObject
    )
    session.add(newShape)
    session.commit()
    filename = f"{newId}.png"
    file.save(os.path.join(app.config['PREVIEW_FOLDER'], filename))
    return ''

if __name__ == "__main__":
    app.run(debug=True)
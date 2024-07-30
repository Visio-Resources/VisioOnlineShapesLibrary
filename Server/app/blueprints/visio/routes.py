from flask import render_template, request, send_file, jsonify, current_app
from app.blueprints.visio import bp
from app.extensions import db, http_auth
from flask_login import current_user
from app.models.visio import Shape, Stencil, ShapeDownload, StencilDownload
from app.utilities import register_shape
from sqlalchemy import func
import json, logging
from pathlib import Path


@bp.route('/get_shapes')
def get_shapes():
    shapes = Shape.query.all()
    return jsonify([shape.serialize() for shape in shapes])


@bp.route('/search', methods=['GET','POST'])
def search():
    if request.method == 'POST':
        search = request.form['search']
    else:
        search = ''
    shapes = Shape.query.where(Shape.name.contains(search)).order_by(Shape.id)
    return render_template('visio/index.html', shapes=shapes)


@bp.route('/download_stencil/<int:stencil_id>')
def download_stencil(stencil_id):
    if  current_user.is_authenticated:
        stencil = Stencil.query.get(stencil_id)
        
        file_path = Path(current_app.root_path) / 'stencils' / f'{stencil_id}{Path(stencil.file_name).suffix}'

        download = StencilDownload(
            stencil_id = stencil_id,
            user_id = current_user.id
        )
        
        db.session.add(download)
        db.session.commit()

        return send_file(file_path, download_name = stencil.file_name, as_attachment=True)
    else:
        return '<p>Downloads are only available for registered users.</p>'


@bp.route('/get_shape/<int:shape_id>')
def get_shape(shape_id):
    if current_user.is_authenticated:
        shape = Shape.query.get(shape_id)

        download = ShapeDownload(
            shape_id = shape_id,
            user_id = current_user.id
        )
        
        db.session.add(download)
        db.session.commit()

        return shape.data_object
    else:
        return register_shape


@bp.route('/add_shape', methods=['POST'])
@http_auth.login_required
def add_shape():
    try:
        add_shape_request = json.loads(request.form['json'])
        file = request.files['image']

        new_shape = Shape(
            name = add_shape_request['Name'],
            prompt = add_shape_request['Prompt'],
            keywords = add_shape_request['Keywords'],
            data_object = add_shape_request['DataObject'],
            user_id = http_auth.current_user().id
        )
        
        db.session.add(new_shape)
        db.session.commit()
        
        file.save(Path(current_app.root_path) / 'static' / 'images' / 'shapes' / f'{new_shape.id}.png')

    except Exception as e:
        logging.exception('Error adding shape.')
        return jsonify({'message': 'Failed'}), 500
    
    return jsonify({'message': 'Success'}), 201


@bp.route('/add_stencil', methods=['POST'])
@http_auth.login_required
def add_stencil():
    try:
        add_stencil_request = json.loads(request.form['json'])
        stencil = request.files['stencil']
        images = request.files.getlist('images')

        i = 0
        shape_id = db.session.query(func.max(Shape.id)).scalar() + 1
        shapes_list = []

        for shape in add_stencil_request['Shapes']:
            image = images[i]
            
            shapes_list.append(Shape(
                id = shape_id,
                name = shape['Name'],
                prompt = shape['Prompt'],
                keywords = shape['Keywords'],
                data_object = shape['DataObject'],
                user_id = http_auth.current_user().id
            ))
            
            image.save(Path(current_app.root_path) / 'static' / 'images' / 'shapes' / f'{shape_id}.png')

            i += 1
            shape_id += 1
        
        new_stencil = Stencil(
            file_name = add_stencil_request['FileName'],
            title = add_stencil_request['Title'],
            subject = add_stencil_request['Subject'],
            author = add_stencil_request['Author'],
            manager = add_stencil_request['Manager'],
            company = add_stencil_request['Company'],
            language = add_stencil_request['Language'],
            categories = add_stencil_request['Categories'],
            tags = add_stencil_request['Tags'],
            comments = add_stencil_request['Comments'],
            shapes = shapes_list,
            user_id = http_auth.current_user().id
        )
        
        db.session.add(new_stencil)
        db.session.commit()

        stencil.save(Path(current_app.root_path) / 'stencils' / f'{new_stencil.id}{Path(stencil.filename).suffix}')

    except Exception:
        logging.exception('Error adding stencil.')
        return jsonify({'message': 'Failed'}), 500
    
    return jsonify({'message': 'Success'}), 201
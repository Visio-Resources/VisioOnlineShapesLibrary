from flask import Blueprint

bp = Blueprint('visio', __name__)


from app.blueprints.visio import routes

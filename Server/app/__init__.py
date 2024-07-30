from flask import Flask, render_template
from config import Config
from app.extensions import db, migrate, bcrypt, login_manager, http_auth, mail, cors

def create_app(config_class=Config):
    app = Flask(__name__)
    app.config.from_object(config_class)

    # Initialize Flask extensions here
    db.init_app(app)
    migrate.init_app(app, db)
    bcrypt.init_app(app)
    login_manager.init_app(app)
    mail.init_app(app)
    cors.init_app(app)

    # Decorators for user authentication
    from app.models.auth import User

    @login_manager.user_loader
    def user_loader(id):
        return User.query.get(int(id))
    
    @http_auth.verify_token
    def verify_token(token):
        return User.query.filter_by(token=token).first()

    # Register blueprints here
    from app.blueprints.visio import bp as visio_bp
    app.register_blueprint(visio_bp)

    from app.blueprints.auth import bp as auth_bp
    app.register_blueprint(auth_bp)

    # Serve SPA
    @app.route('/', defaults={'path': ''})
    @app.route('/<path:path>')
    def index(path):
        if path == '':
            return render_template('index.html')
        if path == 'spa/index.html':
            return render_template(path)
        elif path == 'spa/about.html':
            return render_template(path)
        elif path.startswith('spa/'):
            return render_template("spa/404.html")
        else:
            return render_template("index.html")

    return app
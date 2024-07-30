from flask import render_template, request, url_for, redirect, flash
from app.blueprints.auth import bp
from app.models.auth import User, Team, Role
from app.extensions import db, bcrypt, mail
from flask_login import login_user, login_required, logout_user, current_user
from app.utilities import generate_passord, delete_user_if_not_loggedIn_after_time
from flask_mail import Message
from sqlalchemy import func


@bp.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        email = request.form['email']
        password = request.form['password']
        user : User = User.query.filter_by(email=email).first()
        
        if user:
            if bcrypt.check_password_hash(user.password_hash, password):
                flash('Logged in successfully!', category='success')
                user.last_active = func.now()
                db.session.commit()
                login_user(user, remember=True)
                return redirect(url_for('index'))
            
        flash('Login failed, try again.', category='error')

    return render_template('auth/login.html')


@bp.route('/token_login', methods=['POST'])
def token_login():
    if current_user.is_authenticated:
        logout_user()
        
    token = request.form['token']
    user : User = User.query.filter_by(token=token).first()
    
    if user:
        user.last_active = func.now()
        db.session.commit()
        login_user(user, remember=True)

    return redirect(url_for('index'))


@bp.route('/logout')
@login_required
def logout():
    logout_user()
    return redirect(url_for('auth.login'))


@bp.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'POST':
        email = request.form['email']
        name = request.form['name']

        user = User.query.filter_by(email=email).first()
        if user:
            flash('Email already exists.', category='error')
            return render_template('auth/register.html')
        
        user = User.query.filter_by(name=name).first()
        if user:
            flash('Name already exists.', category='error')
            return render_template('auth/register.html')

        if len(name) < 2:
            flash('Name must be at least 2 characters.', category='error')
            return render_template('auth/register.html')
        
        password = generate_passord(10)
        token = '#' + password
        
        new_user = User(
            email=email,
            name=name,
            password_hash=bcrypt.generate_password_hash(password),
            token=token
        )

        db.session.add(new_user)
        db.session.commit()
        
        msg = Message(
            'Welcome to visio-shapes.com',
            recipients=[email],
            body = (f'You are subscribing to https://www.visio-shapes.com\n'
                f'Please login within the next 5 minutes.\n'
                f'\n'
                f'  Your password is: {password}\n'
                f'\n'
                f'Don\'t worry if you missed the time.\n'
                f'Just register again: https://www.visio-shapes/register\n'
                f'\n'
                f'You did not expect this email?\n'
                f'Someone probably made a typo -> Do nothing.')
        )
        mail.send(msg)

        flash(f'An email has been send to {email}\nPlease login within the next 5 minutes.', category='success')
        delete_user_if_not_loggedIn_after_time(new_user.id)
        return redirect(url_for('auth.login'))

    return render_template('auth/register.html')
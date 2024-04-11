# Visio Online Shapes Library
This is a mockup to brainstorm how such an online library could look like and how it could be implemented.

On server side we have two python modules:
* app.py:
  - Implements the web-app (using Flask)
* watchUploads.py
  - Watches the upload folder for new stecils
  - Exports shape previews
  - Extracts stencil and master properties
  - Creates and fills the (SQLite) database

On client side we have a Visio AddIn for seamless integration. But also the browser can be used to explore the shape library.
Click on the preview will oben the full size picture.
Click on the stencil will start the download and increment the download counter.

## Brainstrorming:
* Drag & drop from AddIn to Visio (is this possible)
  - Other possibilities:
  - JavaSript -> copy to clipboard; AddIn -> paste button?
* Store shapes as DataObject in DB?
* Build stencils from selected shapes on demand?
* Shape previews in higher resolution, maybe SVG?
* Better search / filter functions
* Process uploaded stencils without Visio interop?
  - Should work on linux servers
  - VSSX is zipped XML structure
* Comments, Voting (maybe download counter is enough)?
* Access rights requred?
* License for contributed stencils?
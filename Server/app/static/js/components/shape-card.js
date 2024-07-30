class ShapeCard extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open"});
    }

    connectedCallback() {
        this.render();
        const shape = JSON.parse(this.getAttribute('shape'));
        const div = this.shadowRoot.getElementById(shape.id);
        div.addEventListener("mousedown", (e) => {
            if (e.button != 0) return;
            this.addDataObject();
        });
    }

    static get observedAttributes() {
        return ['shape'];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === 'shape') {
            //this.render();
        }
    }

    render() {
        const shape = JSON.parse(this.getAttribute('shape'));

        this.shadowRoot.innerHTML = `
            <style>
                .stencilContainer {
                    border: 1px solid rgba(0, 0, 0, 0.2);
                    padding: 16px;
                    border-radius: 8px;
                    margin: 8px;
                    display: flex;
                    gap: 8px;
                }

                .stencilPreview {
                    width: 200px;
                }

                .stencilImage {
                    display: block;
                    max-width: 150px;
                    max-height: 500px;
                }

                .stencilText {
                    display: flex;
                    flex-direction: column;
                }

                .stencilName {
                    font-size: large;
                }

                .stencilPrompt {
                    font-size: small;
                }

                .stencilKeywords {
                    font-size: small;
                }

                .stencilDownload {
                    font-size: small;
                }

                .stencilUploadDate {
                    color: rgba(0, 0, 0, 0.4);
                    font-size: smaller;
                }
            </style>

            <div class="stencilContainer">
                <div class="stencilPreview" id="${shape.id}">
                    <a href="static/images/shapes/${shape.id}.png"> <img class="stencilImage" src="static/images/shapes/${shape.id}.png" alt="preview"> </a>
                </div>
                <div class="stencilText">
                    <div class="stencilName"> ${shape.name} </div>
                    <div class="stencilPrompt"> ${shape.prompt} </div>
                    <div class="stencilKeywords"> ${shape.keywords} </div>
                    ${shape.stencil_id != '' ? 
                    `<div class="stencilDownload">
                        <a href="/download_stencil/${shape.stencil_id}">${shape.stencil_file_name}</a>
                    </div>`
                    : ''}
                </div>
            </div>
        `;
    }

    addDataObject() {
        const WebViewDragDrop = window.chrome.webview.hostObjects.WebViewDragDrop;
        const shape = JSON.parse(this.getAttribute('shape'));

        if (shape.data_object) {
            WebViewDragDrop.DragDropShape(shape.data_object);
        } else {
            fetch(`/get_shape/${shape.id}`)
                .then(response => response.text())
                .then(text => {
                    shape.data_object = text;
                    WebViewDragDrop.DragDropShape(shape.data_object);
                    this.setAttribute('shape', JSON.stringify(shape));
                    this.dispatchEvent(new CustomEvent('dataObjectAdded', {
                        detail: { shape },
                        bubbles: true,
                        composed: true
                    }));
                });
        }
    }
}

customElements.define("shape-card", ShapeCard);
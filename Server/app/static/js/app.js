let shapes = [];
let filteredShapes = [];
let isLoading = false;
let noShapesDisplayed = 0;

const searchInput = document.getElementById('search');
const categoryFilter = document.getElementById('category-filter');
const sortOrder = document.getElementById('sort-order');
const shapesContainer = document.getElementById('shapes-container');

const displayNextShapes = () => {
    isLoading = true;
    for (let i = 0; i < 10; i++) {
        if (noShapesDisplayed >= filteredShapes.length) break;
        const shape = filteredShapes[noShapesDisplayed];
        const card = document.createElement('shape-card');
        card.setAttribute('id', shape.id);
        card.setAttribute('shape', JSON.stringify(shape));
        shapesContainer.appendChild(card);
        noShapesDisplayed++;

        card.addEventListener('dataObjectAdded', (event) => {
            const updatedShape = event.detail.shape;
            const index = shapes.findIndex(shape => shape.id === updatedShape.id);
            if (index !== -1) {
                shapes[index] = updatedShape;
            }
        });
    }
    isLoading = false;
}

const filterAndSortShapes = () => {
    const searchTerm = searchInput.value.toLowerCase();
    const selectedCategory = categoryFilter.value;
    const sortOrderValue = sortOrder.value;

    filteredShapes = shapes.filter(shape => {
        const matchesName = shape.name.toLowerCase().includes(searchTerm);
        const matchesCategory = selectedCategory ? shape.categories.includes(selectedCategory) : true;
        return matchesName && matchesCategory;
    });

    filteredShapes.sort((a, b) => {
        if (sortOrderValue.includes('rating')) {
            return sortOrderValue === 'rating-asc' ? a.rating - b.rating : b.rating - a.rating;
        } else {
            return sortOrderValue === 'date-asc' 
                ? new Date(a.upload_date) - new Date(b.upload_date) 
                : new Date(b.upload_date) - new Date(a.upload_date);
        }
    });

    shapesContainer.innerHTML = '';
    noShapesDisplayed = 0;
    displayNextShapes();
};

const getShapes = async () => {
    const response = await fetch('/get_shapes');
    shapes = await response.json();
    filterAndSortShapes();
}

const populateCategories = () => {
    const categories = new Set();
    shapes.forEach(shape => {
        categories.add(shape.keywords);
    });

    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category;
        option.textContent = category;
        categoryFilter.appendChild(option);
    });
};

searchInput.addEventListener('input', filterAndSortShapes);
categoryFilter.addEventListener('change', filterAndSortShapes);
sortOrder.addEventListener('change', filterAndSortShapes);

window.addEventListener('scroll', () => {
    const {
        scrollTop,
        scrollHeight,
        clientHeight
    } = document.documentElement;

    if (scrollTop + clientHeight >= scrollHeight - 5 && !isLoading) {
        displayNextShapes();
    }
});

getShapes();

populateCategories();
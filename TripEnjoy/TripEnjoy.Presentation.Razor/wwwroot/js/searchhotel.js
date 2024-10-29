//selectedCategory
function selectedCategory(element, CategoryId) {
    document.getElementById("CategoryId").value = CategoryId;
    const categories = document.querySelectorAll('.category-oval');
    categories.forEach((category) => {
        category.classList.remove('category-selected');
    });

    element.classList.add('category-selected');
}



document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.getElementById('searchProducts');
    const tableBody = document.querySelector('table tbody');
    const thElements = document.querySelectorAll('table th');

    let products = Array.from(tableBody.getElementsByTagName('tr')).map(row => ({
        element: row,
        data: Array.from(row.cells).map(cell => cell.textContent)
    }));

    const sortTable = (columnIndex, ascending = true) => {
        products.sort((a, b) => {
            const aValue = a.data[columnIndex];
            const bValue = b.data[columnIndex];
            return ascending ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
        });
        products.forEach(product => tableBody.appendChild(product.element));
    };

    thElements.forEach((th, index) => {
        th.addEventListener('click', () => {
            const ascending = th.dataset.sortDirection !== 'asc';
            th.dataset.sortDirection = ascending ? 'asc' : 'desc';
            sortTable(index, ascending);
        });
    });

    searchInput.addEventListener('input', () => {
        const searchTerm = searchInput.value.toLowerCase();
        products.forEach(product => {
            const matches = product.data.some(cell => cell.toLowerCase().includes(searchTerm));
            product.element.style.display = matches ? '' : 'none';
        });
    });
});
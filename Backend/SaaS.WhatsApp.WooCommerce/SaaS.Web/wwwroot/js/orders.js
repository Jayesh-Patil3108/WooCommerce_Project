document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.getElementById('searchInput');
    const tableBody = document.getElementById('ordersTableBody');
    const thElements = document.querySelectorAll('.orders-table th');

    let orders = Array.from(tableBody.getElementsByTagName('tr')).map(row => ({
        element: row,
        data: Array.from(row.cells).map(cell => cell.textContent)
    }));

    const sortTable = (columnIndex, ascending = true) => {
        orders.sort((a, b) => {
            const aValue = a.data[columnIndex];
            const bValue = b.data[columnIndex];
            return ascending ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
        });
        orders.forEach(order => tableBody.appendChild(order.element));
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
        orders.forEach(order => {
            const matches = order.data.some(cell => cell.toLowerCase().includes(searchTerm));
            order.element.style.display = matches ? '' : 'none';
        });
    });
});
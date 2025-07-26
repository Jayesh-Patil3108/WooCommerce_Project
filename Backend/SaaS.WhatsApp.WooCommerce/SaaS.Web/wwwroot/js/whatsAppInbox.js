document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.getElementById('searchWhatsAppInbox');
    const tableBody = document.querySelector('table tbody');
    const thElements = document.querySelectorAll('table th');

    let messages = Array.from(tableBody.getElementsByTagName('tr')).map(row => ({
        element: row,
        data: Array.from(row.cells).map(cell => cell.textContent)
    }));

    const sortTable = (columnIndex, ascending = true) => {
        messages.sort((a, b) => {
            const aValue = a.data[columnIndex];
            const bValue = b.data[columnIndex];
            return ascending ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
        });
        messages.forEach(message => tableBody.appendChild(message.element));
    };

    thElements.forEach((th, index) => {
        th.addEventListener('click', () => {
            const ascending = th.dataset.sortDirection !== 'asc';
            th.dataset.sortDirection = ascending ? 'asc' : 'desc';
            sortTable(index, ascending);
        });
    });

    if (searchInput) {
        searchInput.addEventListener('input', () => {
            const searchTerm = searchInput.value.toLowerCase();
            messages.forEach(message => {
                const matches = message.data.some(cell => cell.toLowerCase().includes(searchTerm));
                message.element.style.display = matches ? '' : 'none';
            });
        });
    }
});
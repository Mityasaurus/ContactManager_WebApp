document.addEventListener('DOMContentLoaded', function () {
    const table = document.getElementById('contactsTable');
    const filterName = document.getElementById('filterName');
    const filterDateOfBirth = document.getElementById('filterDateOfBirth');
    const filterMarried = document.getElementById('filterMarried');
    const filterPhone = document.getElementById('filterPhone');
    const filterSalary = document.getElementById('filterSalary');
    const headers = table.querySelectorAll('thead th');
    let sortColumn = null;
    let sortOrder = 'asc';

    function filterTable() {
        const name = filterName.value.toLowerCase();
        const dateOfBirth = filterDateOfBirth.value.toLowerCase();
        const married = filterMarried.value;
        const phone = filterPhone.value.toLowerCase();
        const salary = filterSalary.value.toLowerCase();

        const rows = Array.from(table.querySelectorAll('tbody tr'));

        rows.forEach(row => {
            const cells = row.children;
            const nameCell = cells[2].querySelector('.view-mode').textContent.toLowerCase();
            const dateOfBirthCell = cells[3].querySelector('.view-mode').textContent.toLowerCase();
            const marriedCell = cells[4].querySelector('.view-mode').textContent.trim();
            const phoneCell = cells[5].querySelector('.view-mode').textContent.toLowerCase();
            const salaryCell = cells[6].querySelector('.view-mode').textContent.toLowerCase();

            const isMatch =
                (!name || nameCell.includes(name)) &&
                (!dateOfBirth || dateOfBirthCell.includes(dateOfBirth)) &&
                (!married || marriedCell === married) &&
                (!phone || phoneCell.includes(phone)) &&
                (!salary || salaryCell.includes(salary));

            row.style.display = isMatch ? '' : 'none';
        });

        sortRows();
    }

    function sortRows() {
        if (sortColumn === null) return;

        const rows = Array.from(table.querySelectorAll('tbody tr'));
        const columnIndex = Array.from(headers).findIndex(th => th.getAttribute('data-column') === sortColumn);

        rows.sort((a, b) => {
            const aText = a.children[columnIndex + 2].querySelector('.view-mode').textContent.trim();
            const bText = b.children[columnIndex + 2].querySelector('.view-mode').textContent.trim();

            if (sortColumn === 'married') {
                return sortOrder === 'asc' ? aText.localeCompare(bText) : bText.localeCompare(aText);
            }

            const aValue = isNaN(aText) ? aText : parseFloat(aText);
            const bValue = isNaN(bText) ? bText : parseFloat(bText);

            if (sortOrder === 'asc') {
                return aValue < bValue ? -1 : aValue > bValue ? 1 : 0;
            } else {
                return aValue > bValue ? -1 : aValue < bValue ? 1 : 0;
            }
        });

        const tbody = table.querySelector('tbody');
        rows.forEach(row => tbody.appendChild(row));
    }

    headers.forEach(header => {
        const p = header.querySelector('th p');
        if (p) {
            p.addEventListener('click', function () {
                const column = header.getAttribute('data-column');
                const currentOrder = header.classList.contains('asc') ? 'asc' : 'desc';
                const newOrder = currentOrder === 'asc' ? 'desc' : 'asc';

                sortColumn = column;
                sortOrder = newOrder;

                headers.forEach(th => th.classList.remove('asc', 'desc'));
                header.classList.add(newOrder);

                filterTable();
            });
        }
    });

    filterName.addEventListener('input', filterTable);
    filterDateOfBirth.addEventListener('input', filterTable);
    filterMarried.addEventListener('change', filterTable);
    filterPhone.addEventListener('input', filterTable);
    filterSalary.addEventListener('input', filterTable);

    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const row = event.target.closest('tr');
            row.querySelectorAll('.view-mode').forEach(element => element.style.display = 'none');
            row.querySelectorAll('.edit-mode').forEach(element => element.style.display = 'block');
        });
    });

    document.querySelectorAll('.cancel-btn').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const row = event.target.closest('tr');
            row.querySelectorAll('.view-mode').forEach(element => element.style.display = 'block');
            row.querySelectorAll('.edit-mode').forEach(element => element.style.display = 'none');
        });
    });

    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function (event) {
            const row = event.target.closest('tr');
            const nameInput = row.querySelector('input[name="Name"]');
            const dateOfBirthInput = row.querySelector('input[name="DateOfBirth"]');
            const phoneInput = row.querySelector('input[name="Phone"]');
            const salaryInput = row.querySelector('input[name="Salary"]');

            let valid = true;

            if (!nameInput.value.trim() || !dateOfBirthInput.value.trim() || !phoneInput.value.trim()) {
                valid = false;
                alert('Name, Date of Birth, and Phone cannot be empty.');
            }

            const phoneValue = phoneInput.value.trim();
            console.log(phoneValue);
            const phoneRegex = /^\+(\d{11,})$/;
            if (!phoneRegex.test(phoneValue)) {
                valid = false;
                alert('Phone must start with a "+" and contain at least 12 characters with digits only.');
            }

            const salaryValue = parseFloat(salaryInput.value);
            if (isNaN(salaryValue) || salaryValue < 0) {
                valid = false;
                alert('Salary must be a positive number.');
            }

            if (!valid) {
                event.preventDefault();
            }
        });
    });
});
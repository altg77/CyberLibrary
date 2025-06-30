document.addEventListener('DOMContentLoaded', function () {
    const tableRows = document.querySelectorAll('tbody tr'); // Seleciona todas as linhas de dados da tabela
    const rowsPerPage = 7; // Quantidade de linhas por página
    let currentPage = 1; // Página atual, começando da 1

    const paginationContainer = document.querySelector('.pagination');
    const prevButton = paginationContainer.querySelector('.prev');
    const nextButton = paginationContainer.querySelector('.next');
    const pageNumberButton = paginationContainer.querySelector('.pagination-button.active'); // O botão do número 1

    function displayRows() {
        const startIndex = (currentPage - 1) * rowsPerPage;
        const endIndex = startIndex + rowsPerPage;

        // Esconde todas as linhas e depois mostra apenas as da página atual
        tableRows.forEach((row, index) => {
            if (index >= startIndex && index < endIndex) {
                row.style.display = ''; // Mostra a linha
            } else {
                row.style.display = 'none'; // Esconde a linha
            }
        });

        // Atualiza o texto do botão de número da página
        if (pageNumberButton) {
            pageNumberButton.textContent = currentPage;
        }

        // Habilita/desabilita botões "Anterior" e "Próximo"
        if (prevButton) {
            prevButton.disabled = currentPage === 1;
        }
        if (nextButton) {
            nextButton.disabled = endIndex >= tableRows.length; // Desabilita se for a última página
        }
    }

    // Event listeners para os botões de paginação
    if (prevButton) {
        prevButton.addEventListener('click', () => {
            if (currentPage > 1) {
                currentPage--;
                displayRows();
            }
        });
    }

    if (nextButton) {
        nextButton.addEventListener('click', () => {
            // Verifica se ainda há mais páginas
            if ((currentPage * rowsPerPage) < tableRows.length) {
                currentPage++;
                displayRows();
            }
        });
    }

    // Inicializa a exibição das linhas ao carregar a página
    displayRows();
});
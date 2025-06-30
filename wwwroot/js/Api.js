document.addEventListener('DOMContentLoaded', function () {
    const isbnInput = document.getElementById('isbnInput');
    const googleBooksSearchBtn = document.getElementById('googleBooksSearchBtn');
    const openLibrarySearchBtn = document.getElementById('openLibrarySearchBtn');

    const previewCapa = document.getElementById('previewCapa');
    const defaultCapaSrc = previewCapa.dataset.defaultSrc;

    const tituloInput = document.getElementById('titulo');
    const autorInput = document.getElementById('autor');
    const editoraInput = document.getElementById('editora');
    const sinopseInput = document.getElementById('sinopse');
    const uploadCapaInput = document.getElementById('uploadCapa'); // Referência ao input de arquivo

    // Não precisamos mais de 'currentCapaFile' globalmente se atribuirmos diretamente.
    // Deixei para referência, mas a nova lógica abaixo o torna menos crítico.

    // Event listener para o botão de reset
    document.getElementById('resetButton').addEventListener('click', function() {
        tituloInput.value = '';
        autorInput.value = '';
        editoraInput.value = '';
        sinopseInput.value = '';
        isbnInput.value = '';

        document.getElementById('categoria').value = '';
        document.getElementById('setor').value = '';

        document.getElementById('quantidade').value = 1;
        document.getElementById('estante').value = '';
        document.getElementById('prateleira').value = '';

        previewCapa.src = defaultCapaSrc;
        uploadCapaInput.value = ''; // Limpa o input de arquivo
        // currentCapaFile = null; // Já não é tão necessário
    });

    // Event listener para o input de upload de capa (do usuário)
    uploadCapaInput.addEventListener('change', function (event) {
        if (event.target.files && event.target.files[0]) {
            const file = event.target.files[0];
            const reader = new FileReader();
            reader.onload = function (e) {
                previewCapa.src = e.target.result;
            };
            reader.readAsDataURL(file);
            // Ao invés de armazenar em currentCapaFile, o arquivo já está no input
        } else {
            previewCapa.src = defaultCapaSrc;
            // currentCapaFile = null;
        }
    });

    async function fetchDataFromOpenLibrary(queryType, queryValue) {
        let apiUrl = '';
        if (queryType === 'isbn') {
            apiUrl = `https://openlibrary.org/api/books?bibkeys=ISBN:${queryValue}&format=json&jscmd=data`;
        } else {
            apiUrl = `https://openlibrary.org/search.json?q=${encodeURIComponent(queryValue)}`;
        }

        try {
            const response = await fetch(apiUrl);
            if (!response.ok) {
                throw new Error(`Erro HTTP! status: ${response.status}`);
            }
            const data = await response.json();
            return data;
        } catch (error) {
            console.error("Erro ao buscar dados da Open Library:", error);
            alert("Erro ao buscar dados. Verifique o ISBN/Termo e sua conexão.");
            return null;
        }
    }

    async function populateFormFields(data, queryType) {
        if (!data) return;

        let capaUrl = null;
        let bookDetails = null;

        // Limpa o input de arquivo e a pré-visualização para cada nova pesquisa
        uploadCapaInput.value = '';
        previewCapa.src = defaultCapaSrc;

        if (queryType === 'isbn') {
            const isbnKey = Object.keys(data)[0];
            if (!isbnKey || !data[isbnKey].details) { // Adicionado verificação para data[isbnKey].details
                alert('Nenhum livro encontrado com este ISBN ou dados detalhados indisponíveis.');
                return;
            }
            bookDetails = data[isbnKey].details;

            tituloInput.value = bookDetails.title || '';
            autorInput.value = bookDetails.authors && bookDetails.authors.length > 0 ? bookDetails.authors[0].name : '';
            editoraInput.value = bookDetails.publishers && bookDetails.publishers.length > 0 ? bookDetails.publishers[0].name : '';
            sinopseInput.value = bookDetails.excerpts && bookDetails.excerpts.length > 0 ? bookDetails.excerpts[0].text : bookDetails.description || '';

            if (bookDetails.covers && bookDetails.covers.length > 0) {
                capaUrl = `https://covers.openlibrary.org/b/id/${bookDetails.covers[0]}-L.jpg`;
            }

        } else { // Pesquisa geral (título/autor)
            if (data.docs && data.docs.length > 0) {
                const book = data.docs[0];

                tituloInput.value = book.title || '';
                autorInput.value = book.author_name && book.author_name.length > 0 ? book.author_name.join(', ') : '';
                editoraInput.value = book.publisher && book.publisher.length > 0 ? book.publisher[0] : '';
                sinopseInput.value = ''; // A sinopse geralmente não vem na pesquisa geral

                if (book.isbn && book.isbn.length > 0) {
                    capaUrl = `https://covers.openlibrary.org/b/isbn/${book.isbn[0]}-L.jpg`;
                } else if (book.cover_i) {
                    capaUrl = `https://covers.openlibrary.org/b/id/${book.cover_i}-L.jpg`;
                }

            } else {
                alert('Nenhum livro encontrado com este termo.');
                return;
            }
        }

        // Se uma capa da API foi encontrada, buscá-la e atribuir ao input de arquivo
        if (capaUrl) {
            try {
                const imageResponse = await fetch(capaUrl);
                if (!imageResponse.ok) {
                    console.error("Erro ao buscar imagem da capa:", imageResponse.statusText);
                    previewCapa.src = defaultCapaSrc;
                    return;
                }
                const imageBlob = await imageResponse.blob();
                const fileName = `capa_${tituloInput.value || 'livro'}.jpg`; // Nome do arquivo baseado no título

                // Cria um novo DataTransfer para atribuir o arquivo ao input de forma programática
                const dataTransfer = new DataTransfer();
                dataTransfer.items.add(new File([imageBlob], fileName, { type: imageBlob.type }));
                uploadCapaInput.files = dataTransfer.files;

                // Exibe a imagem na pré-visualização (lê do input agora)
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewCapa.src = e.target.result;
                };
                reader.readAsDataURL(uploadCapaInput.files[0]);

            } catch (error) {
                console.error("Erro ao processar imagem da capa:", error);
                previewCapa.src = defaultCapaSrc;
                uploadCapaInput.value = '';
            }
        } else {
            previewCapa.src = defaultCapaSrc;
            uploadCapaInput.value = '';
        }
    }

    googleBooksSearchBtn.addEventListener('click', async function () {
        const query = isbnInput.value.trim();
        if (query) {
            if (/^\d{9}[\dX]$|^\d{13}$/.test(query)) {
                const data = await fetchDataFromOpenLibrary('isbn', query);
                await populateFormFields(data, 'isbn');
            } else {
                alert('Por favor, insira um ISBN válido para esta pesquisa.');
            }
        } else {
            alert('Por favor, digite um ISBN ou termo de pesquisa.');
        }
    });

    openLibrarySearchBtn.addEventListener('click', async function () {
        const query = isbnInput.value.trim();
        if (query) {
            const data = await fetchDataFromOpenLibrary('general', query);
            await populateFormFields(data, 'general');
        } else {
            alert('Por favor, digite um termo de pesquisa.');
        }
    });

});
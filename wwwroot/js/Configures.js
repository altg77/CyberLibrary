/* ================= BANNER ================= */
document.addEventListener('DOMContentLoaded', () => {
    const sliderTrack = document.querySelector('.slider-track');
    const dots = document.querySelectorAll('.dot');
    const images = document.querySelectorAll('.slider-track img');
    const heroBanner = document.querySelector('.hero-banner'); // Select the main banner element

    let currentSlide = 0;
    const totalSlides = images.length;
    let autoSlideInterval; // Variable to store the interval ID
    const slideDuration = 5000; // Time in milliseconds (e.g., 5000ms = 5 seconds)

    // Function to update the slider position
    function updateSlider() {
        const slideWidth = images[0].clientWidth;
        sliderTrack.style.transform = `translateX(-${currentSlide * slideWidth}px)`;
        updateDots();
    }

    // Function to update the active dot
    function updateDots() {
        dots.forEach((dot, index) => {
            if (index === currentSlide) {
                dot.classList.add('active');
            } else {
                dot.classList.remove('active');
            }
        });
    }

    // Function to advance to the next slide automatically
    function nextSlide() {
        currentSlide = (currentSlide + 1) % totalSlides;
        updateSlider();
    }

    // Function to start the auto-play
    function startAutoSlide() {
        // Clear any existing interval to prevent multiple intervals running
        clearInterval(autoSlideInterval);
        autoSlideInterval = setInterval(nextSlide, slideDuration);
    }

    // Function to stop the auto-play
    function stopAutoSlide() {
        clearInterval(autoSlideInterval);
    }

    // Event listeners for dots
    dots.forEach(dot => {
        dot.addEventListener('click', (event) => {
            stopAutoSlide(); // Stop auto-play when a dot is clicked
            const slideIndex = parseInt(event.target.dataset.slide);
            currentSlide = slideIndex;
            updateSlider();
            startAutoSlide(); // Restart auto-play after a short delay (optional, but good for user flow)
        });
    });

    // Pause auto-play when mouse enters the banner area
    heroBanner.addEventListener('mouseenter', stopAutoSlide);

    // Resume auto-play when mouse leaves the banner area
    heroBanner.addEventListener('mouseleave', startAutoSlide);

    updateSlider();
    startAutoSlide();

    window.addEventListener('resize', () => {
        updateSlider();
        stopAutoSlide();
        startAutoSlide();
    });
});

/* ================= CARDS ================= */

document.addEventListener('DOMContentLoaded', () => {
    const bookItems = document.querySelectorAll('.book-item');

    bookItems.forEach(item => {
        item.addEventListener('click', () => {
            const parentCarousel = item.closest('.book-carousel');
            const detailedBookView = parentCarousel.nextElementSibling;

            if (detailedBookView && detailedBookView.classList.contains('detailed-book-view')) {
                const detailedBookPath = detailedBookView.querySelector('.detailed-book-path');
                const detailedBookCoverImg = detailedBookView.querySelector('.detailed-book-cover img');
                const detailedBookTitle = detailedBookView.querySelector('.detailed-book-title');
                const detailedBookAuthor = detailedBookView.querySelector('.detailed-book-author');
                const detailedBookDescription = detailedBookView.querySelector('.detailed-book-description');
                const detailedBookTags = detailedBookView.querySelector('.detailed-book-tags'); // Seleciona a div das tags
                const closeButton = detailedBookView.querySelector('.close-detailed-view');

                // Pega os dados dos atributos data-* do item clicado
                const title = item.dataset.title;
                const author = item.dataset.author;
                const description = item.dataset.description;
                const coverSrc = item.dataset.coverUrl;
                const category = item.dataset.category; // Pega a categoria do atributo data-category

                // Preenche os detalhes na visualização detalhada
                detailedBookPath.textContent = title;
                detailedBookCoverImg.src = coverSrc;
                detailedBookCoverImg.alt = `Capa do livro ${title}`;
                detailedBookTitle.textContent = title;
                detailedBookAuthor.textContent = author;
                detailedBookDescription.textContent = description;

                // Limpa as tags existentes antes de adicionar as novas (para evitar duplicidade)
                detailedBookTags.innerHTML = ''; 

                // Adiciona a categoria como uma tag (span)
                if (category) {
                    const categorySpan = document.createElement('span');
                    categorySpan.textContent = category;
                    detailedBookTags.appendChild(categorySpan);
                }

                // Esconde todas as outras visualizações detalhadas que possam estar abertas
                document.querySelectorAll('.detailed-book-view:not(.hidden)').forEach(openView => {
                    openView.classList.add('hidden');
                });

                // Mostra a visualização detalhada correta
                detailedBookView.classList.remove('hidden');

                // Rola a página para a visualização detalhada
                detailedBookView.scrollIntoView({ behavior: 'smooth', block: 'start' });

                // Define o manipulador de clique para o botão de fechar, específico para esta visualização
                closeButton.onclick = () => {
                    detailedBookView.classList.add('hidden');
                };
            }
        });
    });
});
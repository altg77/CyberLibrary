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

document.addEventListener('DOMContentLoaded', function () {
    // Slider functionality (remains unchanged)
    const sliderTrack = document.querySelector('.slider-track');
    const sliderDots = document.querySelectorAll('.slider-dots .dot');
    let currentSlide = 0;

    function updateSlider() {
        if (sliderTrack) {
            sliderTrack.style.transform = `translateX(-${currentSlide * 100}%)`;
        }
        sliderDots.forEach((dot, index) => {
            dot.classList.toggle('active', index === currentSlide);
        });
    }

    sliderDots.forEach(dot => {
        dot.addEventListener('click', function () {
            currentSlide = parseInt(this.dataset.slide);
            updateSlider();
        });
    });

    setInterval(() => {
        currentSlide = (currentSlide + 1) % sliderDots.length;
        updateSlider();
    }, 5000);

    // In-line Detailed Book View functionality
    let currentActiveDetailedView = null; // To keep track of the currently open detail view

    document.querySelectorAll('.book-item').forEach(item => {
        item.addEventListener('click', function () {
            const bookCarouselSection = this.closest('.book-section'); // Get the parent section
            let detailedView = bookCarouselSection.querySelector('.inline-detailed-book-view');

            // If a detailed view is already open in this section, close it
            if (currentActiveDetailedView && currentActiveDetailedView !== detailedView) {
                currentActiveDetailedView.classList.remove('active');
            }
            if (detailedView && detailedView.classList.contains('active') && currentActiveDetailedView === detailedView) {
                // If clicking the same item again, hide it
                detailedView.classList.remove('active');
                currentActiveDetailedView = null;
                return; // Exit function
            }


            // If no detailed view exists, or if it's the first time
            if (!detailedView) {
                detailedView = document.createElement('div');
                detailedView.classList.add('inline-detailed-book-view');
                detailedView.innerHTML = `
                            <button class="close-inline-view">&times;</button>
                            <div class="detailed-cover-col">
                                <img class="detailed-cover-img" src="" alt="Capa do livro">
                            </div>
                            <div class="detailed-info-col">
                                <h2 class="detailed-title"></h2>
                                <p class="detailed-author"></p>
                                <div class="detailed-rating">
                                    <i class="fas fa-star filled"></i><i class="fas fa-star filled"></i><i class="fas fa-star filled"></i><i class="fas fa-star filled"></i><i class="fas fa-star"></i>
                                    <span class="star-count"></span>
                                    <i class="fas fa-share-alt share-icon"></i>
                                </div>
                                <div class="detailed-buttons">
                                    <button class="btn btn-borrow">RESERVAR</button>
                                    <button class="btn btn-preview">PEGAR EMPRESTADO</button>
                                </div>
                                <p class="detailed-description"></p>
                                <div class="detailed-tags">
                                    <span>Fiction</span>
                                </div>
                                <button class="btn-view-more">VER MAIS ></button>
                            </div>
                        `;
                // Insert the new detailed view after the carousel
                bookCarouselSection.appendChild(detailedView);

                // Attach close event listener to the newly created close button
                detailedView.querySelector('.close-inline-view').addEventListener('click', function () {
                    detailedView.classList.remove('active');
                    currentActiveDetailedView = null;
                });
            }

            // Populate and show the detailed view
            const title = this.dataset.title || 'Título Desconhecido';
            const description = this.dataset.description || 'Sem descrição.';
            const imgSrc = this.querySelector('img') ? this.querySelector('img').src : 'https://via.placeholder.com/150';
            const author = this.dataset.author || 'Autor Desconhecido';

            detailedView.querySelector('.detailed-cover-img').src = imgSrc;
            detailedView.querySelector('.detailed-title').textContent = title;
            detailedView.querySelector('.detailed-author').textContent = author;
            detailedView.querySelector('.detailed-description').textContent = description;

            detailedView.classList.add('active'); // Show it
            currentActiveDetailedView = detailedView; // Set as current active
        });
    });

    // Removed the old close button and overlay click listeners as they belong to the modal
});
(function (global) {
    const documentRef = global.document;
    if (!documentRef) {
        return;
    }

    const tryShare = async (button) => {
        const url = button.getAttribute('data-share-url');
        const title = button.getAttribute('data-share-title') || documentRef.title;
        const text = button.getAttribute('data-share-text') || '';

        if (!url) {
            return false;
        }

        if (global.navigator && typeof global.navigator.share === 'function') {
            try {
                await global.navigator.share({ url, title, text });
                return true;
            } catch (error) {
                return false;
            }
        }

        if (global.navigator && global.navigator.clipboard && typeof global.navigator.clipboard.writeText === 'function') {
            try {
                await global.navigator.clipboard.writeText(url);
                return true;
            } catch (error) {
                return false;
            }
        }

        const tempInput = documentRef.createElement('input');
        tempInput.type = 'text';
        tempInput.value = url;
        documentRef.body.appendChild(tempInput);
        tempInput.select();
        try {
            documentRef.execCommand('copy');
            return true;
        } catch (error) {
            return false;
        } finally {
            documentRef.body.removeChild(tempInput);
        }
    };

    const showFeedback = (button, success) => {
        const successLabel = button.getAttribute('data-share-success') || 'لینک کپی شد';
        const errorLabel = button.getAttribute('data-share-error') || 'امکان اشتراک نیست';
        const originalLabel = button.getAttribute('data-original-label') || button.getAttribute('aria-label');
        const feedback = success ? successLabel : errorLabel;

        if (!button.hasAttribute('data-original-label')) {
            button.setAttribute('data-original-label', originalLabel || 'اشتراک‌گذاری');
        }

        button.setAttribute('aria-label', feedback);
        button.classList.add(success ? 'share-success' : 'share-error');

        global.setTimeout(() => {
            button.classList.remove('share-success', 'share-error');
            const storedLabel = button.getAttribute('data-original-label');
            if (storedLabel) {
                button.setAttribute('aria-label', storedLabel);
            }
        }, 1800);
    };

    const initialise = () => {
        const shareButtons = Array.from(documentRef.querySelectorAll('[data-share-url]'));
        shareButtons.forEach((button) => {
            button.addEventListener('click', async (event) => {
                event.preventDefault();
                const result = await tryShare(button);
                showFeedback(button, result);
            });
        });
    };

    if (documentRef.readyState === 'loading') {
        documentRef.addEventListener('DOMContentLoaded', initialise);
    } else {
        initialise();
    }
})(window);

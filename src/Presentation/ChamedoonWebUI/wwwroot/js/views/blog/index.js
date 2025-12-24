(function () {
    const initBlogPage = () => {
        if (window.PersianDatePicker && typeof window.PersianDatePicker.init === 'function') {
            window.PersianDatePicker.init();
        }

        const notifyShare = (button, message) => {
            button.classList.add('is-shared');
            button.setAttribute('data-share-feedback', message);
            window.setTimeout(() => {
                button.classList.remove('is-shared');
                button.removeAttribute('data-share-feedback');
            }, 2000);
        };

        const copyToClipboard = async (text) => {
            if (!text) {
                return false;
            }

            if (navigator.clipboard && window.isSecureContext) {
                try {
                    await navigator.clipboard.writeText(text);
                    return true;
                } catch (error) {
                    // fallback to legacy approach
                }
            }

            try {
                const textArea = document.createElement('textarea');
                textArea.value = text;
                textArea.setAttribute('readonly', '');
                textArea.style.position = 'fixed';
                textArea.style.opacity = '0';

                const container = document.body || document.documentElement;
                container.appendChild(textArea);
                textArea.focus();
                textArea.select();

                const successful = document.execCommand('copy');
                textArea.remove();
                return successful;
            } catch (error) {
                return false;
            }
        };

        document.querySelectorAll('.blog-share-button').forEach((button) => {
            button.addEventListener('click', async () => {
                const url = button.getAttribute('data-share-url');
                const title = button.getAttribute('data-share-title');
                if (!url) {
                    return;
                }

                if (navigator.share) {
                    try {
                        await navigator.share({title, url});
                        notifyShare(button, 'ارسال شد');
                        return;
                    } catch (error) {
                        // کاربر اشتراک‌گذاری را لغو کرده است یا پشتیبانی نمی‌شود
                    }
                }

                if (await copyToClipboard(url)) {
                    notifyShare(button, 'کپی شد');
                    return;
                }

                notifyShare(button, 'لینک: ' + url);
            });
        });
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initBlogPage, {once: true});
    } else {
        initBlogPage();
    }
})();

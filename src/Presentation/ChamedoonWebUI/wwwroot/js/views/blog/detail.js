document.addEventListener('DOMContentLoaded', function () {
    if (window.PersianDatePicker && typeof window.PersianDatePicker.init === 'function') {
        window.PersianDatePicker.init();
    }

    const themeHref = 'https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/styles/github-dark.min.css';
    if (!document.querySelector(`link[href="${themeHref}"]`)) {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = themeHref;
        document.head.appendChild(link);
    }

    const copyText = async (text) => {
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

    document.querySelectorAll('.blog-detail-content pre code').forEach(function (block) {
        hljs.highlightElement(block);

        const pre = block.parentElement;
        if (!pre) {
            return;
        }

        pre.classList.add('code-toolbar');

        if (!pre.querySelector('.code-toolbar-header')) {
            const header = document.createElement('div');
            header.className = 'code-toolbar-header';

            const languageClass = Array.from(block.classList).find(cls => cls.startsWith('language-'));
            const language = languageClass ? languageClass.replace('language-', '').toUpperCase() : 'CODE';

            const badge = document.createElement('span');
            badge.className = 'code-language-badge';
            badge.textContent = language;

            const button = document.createElement('button');
            button.type = 'button';
            button.className = 'copy-code-button';
            button.textContent = 'کپی';

            button.addEventListener('click', async function () {
                const text = block.innerText;
                const copied = await copyText(text);
                if (copied) {
                    const original = button.textContent;
                    button.textContent = 'کپی شد!';
                    setTimeout(function () {
                        button.textContent = original;
                    }, 2000);
                    return;
                }

                button.textContent = 'خطا';
                setTimeout(function () {
                    button.textContent = 'کپی';
                }, 2000);
            });

            header.appendChild(badge);
            header.appendChild(button);
            pre.appendChild(header);
        }
    });
});

(() => {
    const root = document.querySelector('[data-phone-login]');
    if (!root) return;

    const sendForm = document.querySelector('[data-form="send-phone-code"]');
    const sendButton = document.getElementById('sendCodeButton');
    const countdownLabel = document.getElementById('resendCountdown');
    const cooldownSeconds = 120;
    const codeWasSent = root.dataset.codeSent === 'true';

    const formatTime = (seconds) => {
        const mins = Math.floor(seconds / 60);
        const secs = seconds % 60;
        return `${mins}:${secs.toString().padStart(2, '0')}`;
    };

    const updateCountdown = (remaining) => {
        if (countdownLabel) {
            countdownLabel.textContent = `ارسال مجدد کد پس از ${formatTime(remaining)}`;
        }
    };

    const startCooldown = () => {
        if (!sendButton) return;
        let remaining = cooldownSeconds;
        sendButton.disabled = true;
        if (countdownLabel) {
            countdownLabel.style.display = 'block';
            updateCountdown(remaining);
        }

        const intervalId = setInterval(() => {
            remaining -= 1;
            if (remaining <= 0) {
                clearInterval(intervalId);
                sendButton.disabled = false;
                if (countdownLabel) {
                    countdownLabel.style.display = 'none';
                }
                return;
            }

            updateCountdown(remaining);
        }, 1000);
    };

    if (codeWasSent) {
        startCooldown();
    }

    if (sendForm) {
        sendForm.addEventListener('submit', () => {
            if (sendButton) {
                sendButton.disabled = true;
            }
        });
    }
})();

// Notification System
window.showNotification = function(message, type = 'info') {
    const container = document.getElementById('notification-container') || createNotificationContainer();
    
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        <div style="display: flex; align-items: flex-start; gap: 0.5rem;">
            <span style="font-size: 1.2rem;">${getNotificationIcon(type)}</span>
            <div>
                <div style="font-weight: 500; margin-bottom: 0.25rem;">${getNotificationTitle(type)}</div>
                <div style="color: #64748b; font-size: 0.9rem;">${message}</div>
            </div>
            <button onclick="this.parentElement.parentElement.remove()" style="background: none; border: none; cursor: pointer; margin-left: auto; color: #64748b; font-size: 1.2rem;">&times;</button>
        </div>
    `;
    
    container.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentElement) {
            notification.style.animation = 'notificationSlideOut 0.3s ease';
            setTimeout(() => notification.remove(), 300);
        }
    }, 5000);
};

function createNotificationContainer() {
    const container = document.createElement('div');
    container.id = 'notification-container';
    container.className = 'notification-container';
    document.body.appendChild(container);
    return container;
}

function getNotificationIcon(type) {
    switch (type) {
        case 'success': return '✅';
        case 'error': return '❌';
        case 'warning': return '⚠️';
        case 'info': 
        default: return 'ℹ️';
    }
}

function getNotificationTitle(type) {
    switch (type) {
        case 'success': return 'Succès';
        case 'error': return 'Erreur';
        case 'warning': return 'Attention';
        case 'info': 
        default: return 'Information';
    }
}

// Dialog System
window.showConfirmDialog = function(title, message) {
    return new Promise((resolve) => {
        const modal = createModal(title, message, [
            { text: 'Annuler', class: 'btn-secondary', action: () => resolve(false) },
            { text: 'Confirmer', class: 'btn-primary', action: () => resolve(true) }
        ]);
        showModal(modal);
    });
};

window.showInfoDialog = function(title, message) {
    return new Promise((resolve) => {
        const modal = createModal(title, message, [
            { text: 'OK', class: 'btn-primary', action: () => resolve() }
        ]);
        showModal(modal);
    });
};

window.showInputDialog = function(title, message, defaultValue = '') {
    return new Promise((resolve) => {
        const inputHtml = `
            <div style="margin-bottom: 1rem;">${message}</div>
            <input type="text" id="dialog-input" value="${defaultValue}" style="width: 100%; padding: 0.75rem; border: 1px solid #d1d5db; border-radius: 6px; font-size: 1rem;" />
        `;
        
        const modal = createModal(title, inputHtml, [
            { text: 'Annuler', class: 'btn-secondary', action: () => resolve(null) },
            { text: 'OK', class: 'btn-primary', action: () => {
                const input = document.getElementById('dialog-input');
                resolve(input ? input.value : null);
            }}
        ]);
        
        showModal(modal);
        
        // Focus on input after modal is shown
        setTimeout(() => {
            const input = document.getElementById('dialog-input');
            if (input) {
                input.focus();
                input.select();
            }
        }, 100);
    });
};

function createModal(title, content, buttons) {
    const modal = document.createElement('div');
    modal.className = 'modal-container';
    modal.innerHTML = `
        <div class="modal-backdrop"></div>
        <div class="modal-content">
            <div class="modal-header">
                <h3>${title}</h3>
                <button class="modal-close">&times;</button>
            </div>
            <div class="modal-body">
                ${content}
            </div>
            <div class="modal-footer">
                ${buttons.map((btn, index) => `
                    <button class="btn ${btn.class}" data-button-index="${index}">${btn.text}</button>
                `).join('')}
            </div>
        </div>
    `;
    
    // Add event listeners
    const backdrop = modal.querySelector('.modal-backdrop');
    const closeBtn = modal.querySelector('.modal-close');
    const actionButtons = modal.querySelectorAll('[data-button-index]');
    
    const closeModal = () => {
        modal.style.animation = 'modalSlideOut 0.3s ease';
        setTimeout(() => modal.remove(), 300);
    };
    
    backdrop.addEventListener('click', closeModal);
    closeBtn.addEventListener('click', closeModal);
    
    actionButtons.forEach((btn, index) => {
        btn.addEventListener('click', () => {
            buttons[index].action();
            closeModal();
        });
    });
    
    return modal;
}

function showModal(modal) {
    document.body.appendChild(modal);
    // Force reflow to ensure animation works
    modal.offsetHeight;
}

// Add slide out animation
const style = document.createElement('style');
style.textContent = `
    @keyframes notificationSlideOut {
        from {
            opacity: 1;
            transform: translateX(0);
        }
        to {
            opacity: 0;
            transform: translateX(100%);
        }
    }
    
    @keyframes modalSlideOut {
        from {
            opacity: 1;
            transform: translateY(0);
        }
        to {
            opacity: 0;
            transform: translateY(-20px);
        }
    }
`;
document.head.appendChild(style); 
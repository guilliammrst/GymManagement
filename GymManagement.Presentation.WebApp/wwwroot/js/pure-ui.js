// Pure UI JavaScript - Notifications & Modals
class PureUI {
    constructor() {
        this.init();
    }

    init() {
        this.createContainers();
        this.bindEvents();
    }

    createContainers() {
        // Créer le conteneur de notifications s'il n'existe pas
        if (!document.getElementById('notification-container')) {
            const container = document.createElement('div');
            container.id = 'notification-container';
            document.body.appendChild(container);
        }

        // Créer le conteneur de modales s'il n'existe pas
        if (!document.getElementById('modal-container')) {
            const container = document.createElement('div');
            container.id = 'modal-container';
            document.body.appendChild(container);
        }
    }

    bindEvents() {
        // Fermer les modales en cliquant sur le backdrop
        document.addEventListener('click', (e) => {
            if (e.target.id === 'modal-container') {
                this.closeModal();
            }
        });

        // Fermer les modales avec Escape
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') {
                this.closeModal();
            }
        });
    }

    // Notifications
    showNotification(message, type = 'info', duration = 5000) {
        const container = document.getElementById('notification-container');
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        
        const id = 'notification-' + Date.now();
        notification.id = id;
        
        notification.innerHTML = `
            <div class="notification-header">
                <span class="notification-title">${this.getNotificationTitle(type)}</span>
                <button class="notification-close" onclick="pureUI.closeNotification('${id}')">&times;</button>
            </div>
            <div class="notification-message">${message}</div>
        `;
        
        container.appendChild(notification);
        
        // Auto-remove after duration
        setTimeout(() => {
            this.closeNotification(id);
        }, duration);
        
        return id;
    }

    closeNotification(id) {
        const notification = document.getElementById(id);
        if (notification) {
            notification.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.parentNode.removeChild(notification);
                }
            }, 300);
        }
    }

    getNotificationTitle(type) {
        switch (type) {
            case 'success': return 'Succès';
            case 'warning': return 'Attention';
            case 'error': return 'Erreur';
            case 'info':
            default: return 'Information';
        }
    }

    // Modales
    showModal(title, content, buttons = []) {
        const container = document.getElementById('modal-container');
        
        const buttonsHtml = buttons.map((btn, index) => 
            `<button class="btn ${btn.class || 'btn-secondary'}" onclick="pureUI.handleModalButton(${index})">${btn.text}</button>`
        ).join('');
        
        container.innerHTML = `
            <div class="modal">
                <div class="modal-header">
                    <h3 class="modal-title">${title}</h3>
                    <button class="modal-close" onclick="pureUI.closeModal()">&times;</button>
                </div>
                <div class="modal-body">
                    ${content}
                </div>
                <div class="modal-footer">
                    ${buttonsHtml}
                </div>
            </div>
        `;
        
        container.style.display = 'flex';
        this.currentModalButtons = buttons;
        
        // Focus sur le premier input s'il existe
        setTimeout(() => {
            const firstInput = container.querySelector('input, textarea, select');
            if (firstInput) {
                firstInput.focus();
            }
        }, 100);
    }

    closeModal() {
        const container = document.getElementById('modal-container');
        if (container) {
            container.style.display = 'none';
            container.innerHTML = '';
            this.currentModalButtons = [];
        }
    }

    handleModalButton(index) {
        if (this.currentModalButtons && this.currentModalButtons[index]) {
            const button = this.currentModalButtons[index];
            if (button.action) {
                button.action();
            }
        }
        this.closeModal();
    }

    // Dialogues spécialisés
    showConfirm(title, message, onConfirm, onCancel) {
        this.showModal(title, `<p>${message}</p>`, [
            {
                text: 'Annuler',
                class: 'btn-outline',
                action: onCancel || (() => {})
            },
            {
                text: 'Confirmer',
                class: 'btn-primary',
                action: onConfirm || (() => {})
            }
        ]);
    }

    showAlert(title, message, onOk) {
        this.showModal(title, `<p>${message}</p>`, [
            {
                text: 'OK',
                class: 'btn-primary',
                action: onOk || (() => {})
            }
        ]);
    }

    showPrompt(title, message, defaultValue = '', onSubmit, onCancel) {
        const inputId = 'prompt-input-' + Date.now();
        const content = `
            <div class="form-group">
                <label class="form-label">${message}</label>
                <input type="text" id="${inputId}" class="form-input" value="${defaultValue}" />
            </div>
        `;
        
        this.showModal(title, content, [
            {
                text: 'Annuler',
                class: 'btn-outline',
                action: onCancel || (() => {})
            },
            {
                text: 'OK',
                class: 'btn-primary',
                action: () => {
                    const input = document.getElementById(inputId);
                    const value = input ? input.value : '';
                    if (onSubmit) onSubmit(value);
                }
            }
        ]);
    }

    // Utilitaires
    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    throttle(func, limit) {
        let inThrottle;
        return function() {
            const args = arguments;
            const context = this;
            if (!inThrottle) {
                func.apply(context, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    }
}

// Initialiser PureUI
const pureUI = new PureUI();

// Fonctions globales pour Blazor
window.showNotification = (message, type) => {
    pureUI.showNotification(message, type);
};

window.showConfirmDialog = (title, message) => {
    return new Promise((resolve) => {
        pureUI.showConfirm(title, message, 
            () => resolve(true), 
            () => resolve(false)
        );
    });
};

window.showInfoDialog = (title, message) => {
    return new Promise((resolve) => {
        pureUI.showAlert(title, message, () => resolve());
    });
};

window.showInputDialog = (title, message, defaultValue) => {
    return new Promise((resolve) => {
        pureUI.showPrompt(title, message, defaultValue || '', 
            (value) => resolve(value), 
            () => resolve(null)
        );
    });
};

// Utilitaires pour les formulaires
window.validateForm = (formElement) => {
    const inputs = formElement.querySelectorAll('input[required], select[required], textarea[required]');
    let isValid = true;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });
    
    return isValid;
};

// Animation utilitaire
window.animateElement = (element, animation, duration = 300) => {
    element.style.animation = `${animation} ${duration}ms ease`;
    setTimeout(() => {
        element.style.animation = '';
    }, duration);
};

console.log('PureUI initialized successfully'); 
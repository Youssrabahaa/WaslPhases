/**
 * Healthcare Matching Platform - Verification Code Page
 * Vanilla JavaScript Implementation
 * 
 * Features:
 * - Code input validation (numbers only, max 6 digits)
 * - Real-time input formatting
 * - Button interactions and feedback
 * - Resend code functionality with cooldown
 * - Change email functionality
 * - Accessibility support
 */

// ============================================
// DOM Elements
// ============================================

const verificationCodeInput = document.getElementById('verification-code');
const verifyButton = document.getElementById('verify-btn');
const resendButton = document.getElementById('resend-btn');
const changeEmailButton = document.getElementById('change-email-btn');

// ============================================
// State Management
// ============================================

const state = {
    code: '',
    resendCooldown: 0,
    isVerifying: false,
    maxAttempts: 3,
    attempts: 0,
};

// ============================================
// Input Validation & Formatting
// ============================================

/**
 * Handle code input - only allow numbers, max 6 digits
 */
verificationCodeInput.addEventListener('input', (e) => {
    let value = e.target.value;
    
    // Remove non-numeric characters
    value = value.replace(/[^0-9]/g, '');
    
    // Limit to 6 digits
    value = value.slice(0, 6);
    
    // Update input and state
    e.target.value = value;
    state.code = value;
    
    // Enable/disable verify button based on input length
    updateVerifyButtonState();
});

/**
 * Handle paste events - validate and format pasted content
 */
verificationCodeInput.addEventListener('paste', (e) => {
    e.preventDefault();
    
    const pastedText = (e.clipboardData || window.clipboardData).getData('text');
    const cleanedText = pastedText.replace(/[^0-9]/g, '').slice(0, 6);
    
    verificationCodeInput.value = cleanedText;
    state.code = cleanedText;
    
    updateVerifyButtonState();
});

/**
 * Handle keyboard events
 */
verificationCodeInput.addEventListener('keydown', (e) => {
    // Allow Enter key to submit
    if (e.key === 'Enter' && state.code.length === 6 && !state.isVerifying) {
        verifyCode();
    }
    
    // Allow backspace, delete, tab, escape, and arrow keys
    const allowedKeys = ['Backspace', 'Delete', 'Tab', 'Escape', 'ArrowLeft', 'ArrowRight'];
    if (!allowedKeys.includes(e.key) && !/^[0-9]$/.test(e.key)) {
        e.preventDefault();
    }
});

// ============================================
// Button State Management
// ============================================

/**
 * Update verify button state based on input
 */
function updateVerifyButtonState() {
    if (state.code.length === 6 && !state.isVerifying) {
        verifyButton.disabled = false;
        verifyButton.style.cursor = 'pointer';
    } else {
        verifyButton.disabled = true;
        verifyButton.style.cursor = 'not-allowed';
    }
}

/**
 * Update resend button state based on cooldown
 */
function updateResendButtonState() {
    if (state.resendCooldown > 0) {
        resendButton.disabled = true;
        resendButton.textContent = `إعادة إرسال الرمز (${state.resendCooldown}ث)`;
        resendButton.style.opacity = '0.6';
    } else {
        resendButton.disabled = false;
        resendButton.textContent = 'إعادة إرسال الرمز';
        resendButton.style.opacity = '1';
    }
}

// ============================================
// Core Functions
// ============================================

/**
 * Verify the entered code
 */
async function verifyCode() {
    if (state.code.length !== 6 || state.isVerifying) {
        return;
    }
    
    state.isVerifying = true;
    state.attempts++;
    
    // Update button state
    verifyButton.disabled = true;
    verifyButton.textContent = 'جاري التحقق...';
    
    try {
        // Simulate API call
        await simulateVerification(state.code);
        
        // Success
        showSuccessMessage();
        verifyButton.textContent = 'تم التحقق بنجاح ✓';
        verifyButton.style.backgroundColor = '#10b981';
        
        // Disable input after successful verification
        verificationCodeInput.disabled = true;
        resendButton.disabled = true;
        changeEmailButton.disabled = true;
        
    } catch (error) {
        // Error
        showErrorMessage(error.message);
        verifyButton.textContent = 'تحقق';
        
        // Check if max attempts reached
        if (state.attempts >= state.maxAttempts) {
            showMaxAttemptsMessage();
            verifyButton.disabled = true;
            verificationCodeInput.disabled = true;
        }
    } finally {
        state.isVerifying = false;
        updateVerifyButtonState();
    }
}

/**
 * Simulate API verification (replace with actual API call)
 */
function simulateVerification(code) {
    return new Promise((resolve, reject) => {
        // Simulate network delay
        setTimeout(() => {
            // Accept code "123456" as valid for demo
            if (code === '123456') {
                resolve({ success: true });
            } else {
                reject(new Error('الرمز غير صحيح. يرجى المحاولة مرة أخرى.'));
            }
        }, 1500);
    });
}

/**
 * Resend verification code
 */
async function resendCode() {
    if (state.resendCooldown > 0) {
        return;
    }
    
    resendButton.disabled = true;
    resendButton.textContent = 'جاري الإرسال...';
    
    try {
        // Simulate API call
        await simulateResend();
        
        // Reset input
        verificationCodeInput.value = '';
        verificationCodeInput.focus();
        state.code = '';
        state.attempts = 0;
        
        // Show success message
        showToast('تم إعادة إرسال الرمز بنجاح', 'success');
        
        // Start cooldown
        startResendCooldown(60);
        
    } catch (error) {
        showToast('فشل إعادة الإرسال. حاول مرة أخرى.', 'error');
        resendButton.textContent = 'إعادة إرسال الرمز';
        resendButton.disabled = false;
    }
}

/**
 * Simulate resend API call
 */
function simulateResend() {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve({ success: true });
        }, 1000);
    });
}

/**
 * Change email address
 */
async function changeEmail() {
    // In a real application, this would navigate to a change email page
    // or open a modal dialog
    
    const newEmail = prompt('أدخل عنوان البريد الإلكتروني الجديد:');
    
    if (newEmail && newEmail.trim()) {
        // Validate email format
        if (!isValidEmail(newEmail)) {
            showToast('صيغة البريد الإلكتروني غير صحيحة', 'error');
            return;
        }
        
        changeEmailButton.disabled = true;
        changeEmailButton.textContent = 'جاري التحديث...';
        
        try {
            // Simulate API call
            await simulateChangeEmail(newEmail);
            
            // Update email display
            const emailElement = document.querySelector('.info-email');
            emailElement.textContent = newEmail;
            
            // Reset form
            verificationCodeInput.value = '';
            state.code = '';
            state.attempts = 0;
            
            showToast('تم تحديث البريد الإلكتروني بنجاح', 'success');
            
        } catch (error) {
            showToast('فشل تحديث البريد الإلكتروني', 'error');
        } finally {
            changeEmailButton.disabled = false;
            changeEmailButton.textContent = 'تغيير البريد الإلكتروني';
        }
    }
}

/**
 * Simulate change email API call
 */
function simulateChangeEmail(email) {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve({ success: true });
        }, 1000);
    });
}

/**
 * Start resend cooldown timer
 */
function startResendCooldown(seconds) {
    state.resendCooldown = seconds;
    updateResendButtonState();
    
    const interval = setInterval(() => {
        state.resendCooldown--;
        updateResendButtonState();
        
        if (state.resendCooldown <= 0) {
            clearInterval(interval);
        }
    }, 1000);
}

// ============================================
// Utility Functions
// ============================================

/**
 * Validate email format
 */
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

/**
 * Show success message
 */
function showSuccessMessage() {
    showToast('تم التحقق من الرمز بنجاح!', 'success');
}

/**
 * Show error message
 */
function showErrorMessage(message) {
    showToast(message, 'error');
}

/**
 * Show max attempts message
 */
function showMaxAttemptsMessage() {
    showToast('تم تجاوز عدد محاولات التحقق. يرجى إعادة إرسال الرمز.', 'error');
}

/**
 * Show toast notification
 */
function showToast(message, type = 'info') {
    // Create toast element
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.textContent = message;
    
    // Add styles
    toast.style.cssText = `
        position: fixed;
        bottom: 20px;
        left: 20px;
        right: 20px;
        max-width: 400px;
        padding: 12px 16px;
        background-color: ${type === 'success' ? '#10b981' : type === 'error' ? '#ef4444' : '#3b82f6'};
        color: white;
        border-radius: 8px;
        font-size: 14px;
        font-family: 'Cairo', sans-serif;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        animation: slideUp 0.3s ease-out;
        z-index: 1000;
        text-align: right;
    `;
    
    document.body.appendChild(toast);
    
    // Auto remove after 3 seconds
    setTimeout(() => {
        toast.style.animation = 'slideDown 0.3s ease-out';
        setTimeout(() => {
            toast.remove();
        }, 300);
    }, 3000);
}

// ============================================
// Event Listeners
// ============================================

verifyButton.addEventListener('click', verifyCode);
resendButton.addEventListener('click', resendCode);
changeEmailButton.addEventListener('click', changeEmail);

// Focus input on page load
window.addEventListener('load', () => {
    verificationCodeInput.focus();
});

// ============================================
// CSS Animations (injected)
// ============================================

const style = document.createElement('style');
style.textContent = `
    @keyframes slideUp {
        from {
            opacity: 0;
            transform: translateY(20px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
    
    @keyframes slideDown {
        from {
            opacity: 1;
            transform: translateY(0);
        }
        to {
            opacity: 0;
            transform: translateY(20px);
        }
    }
`;
document.head.appendChild(style);

// ============================================
// Initialization
// ============================================

// Initialize button states
updateVerifyButtonState();
updateResendButtonState();

console.log('Healthcare Verification Page initialized');

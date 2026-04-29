/**
 * Healthcare Matching Platform - Password Reset Form
 * Vanilla JavaScript - No dependencies required
 * 
 * Features:
 * - Email validation
 * - Form submission handling
 * - Loading state management
 * - Toast notifications
 * - Back navigation
 * - Keyboard support
 */

(function() {
    'use strict';

    // ============================================
    // DOM Elements
    // ============================================
    
    const form = document.getElementById('resetPasswordForm');
    const emailInput = document.getElementById('emailInput');
    const emailError = document.getElementById('emailError');
    const submitBtn = document.getElementById('submitBtn');
    const btnText = document.querySelector('.btn-text');
    const btnLoader = document.getElementById('btnLoader');
    const backLink = document.getElementById('backLink');
    const successToast = document.getElementById('successToast');
    const toastMessage = document.getElementById('toastMessage');

    // ============================================
    // Configuration
    // ============================================

    const CONFIG = {
        API_ENDPOINT: '/api/password-reset', // Change this to your .NET API endpoint
        TOAST_DURATION: 4000,
        DEBOUNCE_DELAY: 300,
        MIN_EMAIL_LENGTH: 5,
        MAX_EMAIL_LENGTH: 254
    };

    // ============================================
    // Utility Functions
    // ============================================

    /**
     * Validate email format
     */
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email) && 
               email.length >= CONFIG.MIN_EMAIL_LENGTH && 
               email.length <= CONFIG.MAX_EMAIL_LENGTH;
    }

    /**
     * Show error message
     */
    function showError(message) {
        emailError.textContent = message;
        emailInput.classList.add('error');
    }

    /**
     * Clear error message
     */
    function clearError() {
        emailError.textContent = '';
        emailInput.classList.remove('error');
    }

    /**
     * Show toast notification
     */
    function showToast(message, type = 'success') {
        toastMessage.textContent = message;
        successToast.className = `toast show ${type}`;

        setTimeout(() => {
            successToast.classList.remove('show');
        }, CONFIG.TOAST_DURATION);
    }

    /**
     * Set loading state
     */
    function setLoading(isLoading) {
        submitBtn.disabled = isLoading;
        
        if (isLoading) {
            btnText.style.display = 'none';
            btnLoader.style.display = 'inline-block';
        } else {
            btnText.style.display = 'inline';
            btnLoader.style.display = 'none';
        }
    }

    /**
     * Debounce function
     */
    function debounce(func, delay) {
        let timeoutId;
        return function(...args) {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => func.apply(this, args), delay);
        };
    }

    // ============================================
    // Event Handlers
    // ============================================

    /**
     * Email input validation on input
     */
    const validateEmailInput = debounce(function() {
        const email = emailInput.value.trim();

        if (!email) {
            clearError();
            return;
        }

        if (!isValidEmail(email)) {
            showError('البريد الإلكتروني غير صحيح');
        } else {
            clearError();
        }
    }, CONFIG.DEBOUNCE_DELAY);

    emailInput.addEventListener('input', validateEmailInput);

    /**
     * Clear error on focus
     */
    emailInput.addEventListener('focus', clearError);

    /**
     * Form submission
     */
    form.addEventListener('submit', async function(e) {
        e.preventDefault();

        const email = emailInput.value.trim();

        // Validate email
        if (!email) {
            showError('البريد الإلكتروني مطلوب');
            return;
        }

        if (!isValidEmail(email)) {
            showError('البريد الإلكتروني غير صحيح');
            return;
        }

        // Set loading state
        setLoading(true);

        try {
            // Send request to your .NET API
            const response = await fetch(CONFIG.API_ENDPOINT, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    // Add CSRF token if needed for .NET MVC
                    // 'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    email: email
                })
            });

            // Handle response
            if (response.ok) {
                const data = await response.json();
                
                // Success
                showToast('تم إرسال رمز التحقق بنجاح. تحقق من بريدك الإلكتروني');
                
                // Clear form
                emailInput.value = '';
                clearError();

                // Optional: Redirect to verification page after delay
                // setTimeout(() => {
                //     window.location.href = '/verify-code';
                // }, 2000);

            } else {
                // Handle error response
                const error = await response.json();
                showToast(error.message || 'حدث خطأ. يرجى المحاولة مرة أخرى', 'error');
            }

        } catch (error) {
            // Network error or other issues
            console.error('Error:', error);
            showToast('حدث خطأ في الاتصال. يرجى التحقق من الإنترنت والمحاولة مرة أخرى', 'error');

        } finally {
            // Clear loading state
            setLoading(false);
        }
    });

    /**
     * Back to login link
     */
    backLink.addEventListener('click', function(e) {
        e.preventDefault();
        
        // Option 1: Go back to previous page
        window.history.back();
        
        // Option 2: Redirect to login page (uncomment if needed)
        // window.location.href = '/login';
    });

    // ============================================
    // Keyboard Support
    // ============================================

    /**
     * Submit form on Enter key
     */
    emailInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            form.dispatchEvent(new Event('submit'));
        }
    });

    // ============================================
    // Initialization
    // ============================================

    /**
     * Set initial focus on email input
     */
    window.addEventListener('load', function() {
        emailInput.focus();
    });

    // ============================================
    // Public API (for external use in .NET)
    // ============================================

    /**
     * Expose public methods for .NET MVC integration
     */
    window.PasswordResetForm = {
        /**
         * Submit form programmatically
         */
        submit: function() {
            form.dispatchEvent(new Event('submit'));
        },

        /**
         * Set email value
         */
        setEmail: function(email) {
            emailInput.value = email;
            validateEmailInput();
        },

        /**
         * Get email value
         */
        getEmail: function() {
            return emailInput.value.trim();
        },

        /**
         * Reset form
         */
        reset: function() {
            form.reset();
            clearError();
            setLoading(false);
        },

        /**
         * Set custom API endpoint
         */
        setApiEndpoint: function(endpoint) {
            CONFIG.API_ENDPOINT = endpoint;
        },

        /**
         * Show custom message
         */
        showMessage: function(message, type = 'success') {
            showToast(message, type);
        },

        /**
         * Set form loading state
         */
        setLoading: function(isLoading) {
            setLoading(isLoading);
        },

        /**
         * Disable/Enable form
         */
        setDisabled: function(disabled) {
            emailInput.disabled = disabled;
            submitBtn.disabled = disabled;
            backLink.disabled = disabled;
        }
    };

})();

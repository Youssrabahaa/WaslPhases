/**
 * Healthcare Matching Platform - Student Registration Form
 * Vanilla JavaScript - No dependencies required
 * 
 * Features:
 * - Full name validation
 * - Phone number validation
 * - Email validation
 * - Form submission handling
 * - Toast notifications
 * - Back button navigation
 * - Keyboard support
 */

(function() {
    'use strict';

    // ============================================
    // DOM Elements
    // ============================================
    
    const form = document.getElementById('registrationForm');
    const fullNameInput = document.getElementById('fullNameInput');
    const phoneInput = document.getElementById('phoneInput');
    const emailInput = document.getElementById('emailInput');
    const fullNameError = document.getElementById('fullNameError');
    const phoneError = document.getElementById('phoneError');
    const emailError = document.getElementById('emailError');
    const submitBtn = document.getElementById('submitBtn');
    const btnText = document.querySelector('.btn-text');
    const btnLoader = document.getElementById('btnLoader');
    const backBtn = document.getElementById('backBtn');
    const successToast = document.getElementById('successToast');
    const toastMessage = document.getElementById('toastMessage');

    // ============================================
    // Configuration
    // ============================================

    const CONFIG = {
        API_ENDPOINT: '/api/register-student', // Change this to your .NET API endpoint
        TOAST_DURATION: 4000,
        DEBOUNCE_DELAY: 300,
        MIN_NAME_LENGTH: 3,
        MAX_NAME_LENGTH: 100,
        MIN_PHONE_LENGTH: 10,
        MAX_PHONE_LENGTH: 20,
        MAX_EMAIL_LENGTH: 254
    };

    // ============================================
    // Utility Functions
    // ============================================

    /**
     * Validate full name
     */
    function isValidFullName(name) {
        const trimmedName = name.trim();
        return trimmedName.length >= CONFIG.MIN_NAME_LENGTH && 
               trimmedName.length <= CONFIG.MAX_NAME_LENGTH &&
               /^[\u0600-\u06FF\s]+$/.test(trimmedName); // Arabic characters only
    }

    /**
     * Validate phone number (Saudi format)
     */
    function isValidPhone(phone) {
        const phoneRegex = /^(\+966|0)?[5][0-9]{8}$/;
        const cleanPhone = phone.replace(/\s/g, '');
        return cleanPhone.length >= CONFIG.MIN_PHONE_LENGTH && 
               cleanPhone.length <= CONFIG.MAX_PHONE_LENGTH &&
               phoneRegex.test(cleanPhone);
    }

    /**
     * Validate email format
     */
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email) && 
               email.length >= 5 && 
               email.length <= CONFIG.MAX_EMAIL_LENGTH;
    }

    /**
     * Show error message
     */
    function showError(inputElement, errorElement, message) {
        errorElement.textContent = message;
        inputElement.classList.add('error');
    }

    /**
     * Clear error message
     */
    function clearError(inputElement, errorElement) {
        errorElement.textContent = '';
        inputElement.classList.remove('error');
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
        backBtn.disabled = isLoading;
        
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
     * Full name input validation on input
     */
    const validateFullNameInput = debounce(function() {
        const fullName = fullNameInput.value.trim();

        if (!fullName) {
            clearError(fullNameInput, fullNameError);
            return;
        }

        if (!isValidFullName(fullName)) {
            showError(fullNameInput, fullNameError, 'الاسم يجب أن يكون بالعربية ولا يقل عن 3 أحرف');
        } else {
            clearError(fullNameInput, fullNameError);
        }
    }, CONFIG.DEBOUNCE_DELAY);

    fullNameInput.addEventListener('input', validateFullNameInput);
    fullNameInput.addEventListener('focus', () => clearError(fullNameInput, fullNameError));

    /**
     * Phone input validation on input
     */
    const validatePhoneInput = debounce(function() {
        const phone = phoneInput.value.trim();

        if (!phone) {
            clearError(phoneInput, phoneError);
            return;
        }

        if (!isValidPhone(phone)) {
            showError(phoneInput, phoneError, 'رقم الجوال غير صحيح (يجب أن يكون رقم سعودي)');
        } else {
            clearError(phoneInput, phoneError);
        }
    }, CONFIG.DEBOUNCE_DELAY);

    phoneInput.addEventListener('input', validatePhoneInput);
    phoneInput.addEventListener('focus', () => clearError(phoneInput, phoneError));

    /**
     * Email input validation on input
     */
    const validateEmailInput = debounce(function() {
        const email = emailInput.value.trim();

        if (!email) {
            clearError(emailInput, emailError);
            return;
        }

        if (!isValidEmail(email)) {
            showError(emailInput, emailError, 'البريد الإلكتروني غير صحيح');
        } else {
            clearError(emailInput, emailError);
        }
    }, CONFIG.DEBOUNCE_DELAY);

    emailInput.addEventListener('input', validateEmailInput);
    emailInput.addEventListener('focus', () => clearError(emailInput, emailError));

    /**
     * Form submission
     */
    form.addEventListener('submit', async function(e) {
        e.preventDefault();

        const fullName = fullNameInput.value.trim();
        const phone = phoneInput.value.trim();
        const email = emailInput.value.trim();

        // Validate all fields
        let isValid = true;

        if (!fullName) {
            showError(fullNameInput, fullNameError, 'الاسم الكامل مطلوب');
            isValid = false;
        } else if (!isValidFullName(fullName)) {
            showError(fullNameInput, fullNameError, 'الاسم يجب أن يكون بالعربية ولا يقل عن 3 أحرف');
            isValid = false;
        }

        if (!phone) {
            showError(phoneInput, phoneError, 'رقم الجوال مطلوب');
            isValid = false;
        } else if (!isValidPhone(phone)) {
            showError(phoneInput, phoneError, 'رقم الجوال غير صحيح');
            isValid = false;
        }

        if (!email) {
            showError(emailInput, emailError, 'البريد الإلكتروني مطلوب');
            isValid = false;
        } else if (!isValidEmail(email)) {
            showError(emailInput, emailError, 'البريد الإلكتروني غير صحيح');
            isValid = false;
        }

        if (!isValid) {
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
                    fullName: fullName,
                    phone: phone,
                    email: email,
                    role: 'student'
                })
            });

            // Handle response
            if (response.ok) {
                const data = await response.json();
                
                // Success
                showToast('تم إنشاء الحساب بنجاح');
                
                // Clear form
                form.reset();
                clearError(fullNameInput, fullNameError);
                clearError(phoneInput, phoneError);
                clearError(emailInput, emailError);

                // Optional: Redirect to next page after delay
                // setTimeout(() => {
                //     window.location.href = data.redirectUrl || '/dashboard';
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
     * Back button click
     */
    backBtn.addEventListener('click', function(e) {
        e.preventDefault();
        
        // Option 1: Go back to previous page
        window.history.back();
        
        // Option 2: Redirect to registration page (uncomment if needed)
        // window.location.href = '/register';
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
     * Set initial focus on full name input
     */
    window.addEventListener('load', function() {
        fullNameInput.focus();
    });

    // ============================================
    // Public API (for external use in .NET)
    // ============================================

    /**
     * Expose public methods for .NET MVC integration
     */
    window.StudentRegistrationForm = {
        /**
         * Submit form programmatically
         */
        submit: function() {
            form.dispatchEvent(new Event('submit'));
        },

        /**
         * Get form data
         */
        getFormData: function() {
            return {
                fullName: fullNameInput.value.trim(),
                phone: phoneInput.value.trim(),
                email: emailInput.value.trim()
            };
        },

        /**
         * Set form data
         */
        setFormData: function(data) {
            if (data.fullName) fullNameInput.value = data.fullName;
            if (data.phone) phoneInput.value = data.phone;
            if (data.email) emailInput.value = data.email;
        },

        /**
         * Reset form
         */
        reset: function() {
            form.reset();
            clearError(fullNameInput, fullNameError);
            clearError(phoneInput, phoneError);
            clearError(emailInput, emailError);
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
            fullNameInput.disabled = disabled;
            phoneInput.disabled = disabled;
            emailInput.disabled = disabled;
            submitBtn.disabled = disabled;
            backBtn.disabled = disabled;
        },

        /**
         * Validate all fields
         */
        validate: function() {
            const fullName = fullNameInput.value.trim();
            const phone = phoneInput.value.trim();
            const email = emailInput.value.trim();

            let isValid = true;

            if (!fullName || !isValidFullName(fullName)) {
                showError(fullNameInput, fullNameError, 'الاسم الكامل غير صحيح');
                isValid = false;
            }

            if (!phone || !isValidPhone(phone)) {
                showError(phoneInput, phoneError, 'رقم الجوال غير صحيح');
                isValid = false;
            }

            if (!email || !isValidEmail(email)) {
                showError(emailInput, emailError, 'البريد الإلكتروني غير صحيح');
                isValid = false;
            }

            return isValid;
        }
    };

})();

/**
 * Healthcare Matching Platform - Registration Page
 * Vanilla JavaScript - No dependencies required
 * 
 * Features:
 * - Role selection (Patient / Student)
 * - Form submission handling
 * - Toast notifications
 * - Login link navigation
 * - Keyboard support
 */

(function() {
    'use strict';

    // ============================================
    // DOM Elements
    // ============================================
    
    const patientBtn = document.getElementById('patientBtn');
    const studentBtn = document.getElementById('studentBtn');
    const loginLink = document.getElementById('loginLink');
    const successToast = document.getElementById('successToast');
    const toastMessage = document.getElementById('toastMessage');

    // ============================================
    // Configuration
    // ============================================

    const CONFIG = {
        API_ENDPOINT: '/api/register', // Change this to your .NET API endpoint
        TOAST_DURATION: 4000,
        SELECTED_CLASS: 'selected'
    };

    // ============================================
    // State Management
    // ============================================

    let selectedRole = null;

    // ============================================
    // Utility Functions
    // ============================================

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
     * Set selected role
     */
    function setSelectedRole(role) {
        selectedRole = role;
        
        // Update button states
        if (role === 'patient') {
            patientBtn.classList.add(CONFIG.SELECTED_CLASS);
            studentBtn.classList.remove(CONFIG.SELECTED_CLASS);
        } else if (role === 'student') {
            studentBtn.classList.add(CONFIG.SELECTED_CLASS);
            patientBtn.classList.remove(CONFIG.SELECTED_CLASS);
        } else {
            patientBtn.classList.remove(CONFIG.SELECTED_CLASS);
            studentBtn.classList.remove(CONFIG.SELECTED_CLASS);
        }
    }

    /**
     * Get selected role
     */
    function getSelectedRole() {
        return selectedRole;
    }

    // ============================================
    // Event Handlers
    // ============================================

    /**
     * Patient button click
     */
    //patientBtn.addEventListener('click', async function(e) {
    //    e.preventDefault();
        
    //    if (selectedRole === 'patient') {
    //        // Deselect if already selected
    //        setSelectedRole(null);
    //    } else {
    //        // Select patient role
    //        setSelectedRole('patient');
            
    //        // Submit registration
    //        await submitRegistration('patient');
    //    }
    //});

    /**
     * Student button click
     */
    studentBtn.addEventListener('click', async function(e) {
        e.preventDefault();
        
        if (selectedRole === 'student') {
            // Deselect if already selected
            setSelectedRole(null);
        } else {
            // Select student role
            setSelectedRole('student');
            
            // Submit registration
            await submitRegistration('student');
        }
    });

    /**
     * Submit registration
     */
    async function submitRegistration(role) {
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
                    role: role
                })
            });

            // Handle response
            if (response.ok) {
                const data = await response.json();
                
                // Success
                showToast(`تم اختيار دور ${role === 'patient' ? 'المريض' : 'طالب طب الأسنان'} بنجاح`);
                
                // Optional: Redirect to next page after delay
                // setTimeout(() => {
                //     window.location.href = data.redirectUrl || '/register/details';
                // }, 1500);

            } else {
                // Handle error response
                const error = await response.json();
                showToast(error.message || 'حدث خطأ. يرجى المحاولة مرة أخرى', 'error');
                setSelectedRole(null);
            }

        } catch (error) {
            // Network error or other issues
            console.error('Error:', error);
            showToast('حدث خطأ في الاتصال. يرجى التحقق من الإنترنت والمحاولة مرة أخرى', 'error');
            setSelectedRole(null);
        }
    }

    /**
     * Login link click
     */
    loginLink.addEventListener('click', function(e) {
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
     * Keyboard navigation
     */
    //document.addEventListener('keydown', function(e) {
    //    if (e.key === 'Enter') {
    //        if (document.activeElement === patientBtn) {
    //            patientBtn.click();
    //        } else if (document.activeElement === studentBtn) {
    //            studentBtn.click();
    //        } else if (document.activeElement === loginLink) {
    //            loginLink.click();
    //        }
    //    }
    //});

    // ============================================
    // Initialization
    // ============================================

    /**
     * Set initial focus on first role button
     */
    window.addEventListener('load', function() {
        patientBtn.focus();
    });

    // ============================================
    // Public API (for external use in .NET)
    // ============================================

    /**
     * Expose public methods for .NET MVC integration
     */
    window.RegistrationForm = {
        /**
         * Get selected role
         */
        getRole: function() {
            return getSelectedRole();
        },

        /**
         * Set role programmatically
         */
        setRole: function(role) {
            setSelectedRole(role);
        },

        /**
         * Submit registration programmatically
         */
        submit: function(role) {
            return submitRegistration(role);
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
         * Clear selection
         */
        clearSelection: function() {
            setSelectedRole(null);
        },

        /**
         * Disable/Enable buttons
         */
        setDisabled: function(disabled) {
            patientBtn.disabled = disabled;
            studentBtn.disabled = disabled;
            loginLink.disabled = disabled;
        }
    };

})();

// Form Elements
const emailInput = document.getElementById('emailInput');
const passwordInput = document.getElementById('passwordInput');
const loginBtn = document.getElementById('loginBtn');
const forgotPasswordLink = document.querySelector('.forgot-password-link');
const signupLink = document.querySelector('.signup-link');

// Email validation regex
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// Initialize event listeners
document.addEventListener('DOMContentLoaded', function() {
    setupEventListeners();
});

function setupEventListeners() {
    // Login button click
    loginBtn.addEventListener('click', handleLogin);

    // Enter key in password field
    passwordInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            handleLogin();
        }
    });

   
    // Signup link
    signupLink.addEventListener('click', function(e) {
        e.preventDefault();
        handleSignup();
    });

    // Input validation on change
    emailInput.addEventListener('input', validateEmail);
    passwordInput.addEventListener('input', validatePassword);
}

function validateEmail() {
    const email = emailInput.value.trim();
    if (email && !emailRegex.test(email)) {
        emailInput.classList.add('error');
        return false;
    } else {
        emailInput.classList.remove('error');
        return true;
    }
}

function validatePassword() {
    const password = passwordInput.value;
    if (password && password.length < 6) {
        passwordInput.classList.add('error');
        return false;
    } else {
        passwordInput.classList.remove('error');
        return true;
    }
}

function handleLogin() {
    const email = emailInput.value.trim();
    const password = passwordInput.value;

    // Validation
    if (!email) {
        showError('البريد الإلكتروني مطلوب');
        emailInput.focus();
        return;
    }

    if (!emailRegex.test(email)) {
        showError('البريد الإلكتروني غير صحيح');
        emailInput.focus();
        return;
    }

    if (!password) {
        showError('كلمة المرور مطلوبة');
        passwordInput.focus();
        return;
    }

    if (password.length < 6) {
        showError('كلمة المرور يجب أن تكون 6 أحرف على الأقل');
        passwordInput.focus();
        return;
    }

    // Simulate login
    loginBtn.disabled = true;
    loginBtn.textContent = 'جاري التحميل...';

    setTimeout(() => {
        showSuccess('تم تسجيل الدخول بنجاح!');
        
        // Reset form after 2 seconds
        setTimeout(() => {
            resetForm();
            loginBtn.disabled = false;
            loginBtn.textContent = 'تسجيل الدخول';
        }, 2000);
    }, 1500);
}

https://www.figma.com/design/ks9v0kLQFVmdQxfshq0dNh/Wasl?node-id=27-2406&t=I08QK6zgGdsvTROF-0

function handleSignup() {
    // بدل الـ alert، هنقول للمتصفح يروح للمسار بتاعنا في الـ .NET
    window.location.href = '/Auth/SelectRole';
}

function showError(message) {
    alert(message);
}

function showSuccess(message) {
    alert(message);
}

function resetForm() {
    emailInput.value = '';
    passwordInput.value = '';
    emailInput.classList.remove('error');
    passwordInput.classList.remove('error');
}

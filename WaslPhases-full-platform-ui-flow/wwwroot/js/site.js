document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-loading-form]").forEach((form) => {
    form.addEventListener("submit", () => {
      form.classList.add("is-loading");
      form.querySelectorAll("button[type='submit']").forEach((button) => {
        button.setAttribute("aria-busy", "true");
      });
    });
  });
});

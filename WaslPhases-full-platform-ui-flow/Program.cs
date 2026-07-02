using Microsoft.EntityFrameworkCore;
using phase_1.BLL.Services;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.DAL.Repositories;
using phase_1.Data;
using phase_1.Middleware;
using phase_1.Repositories;
using phase_1.BLL.BackgroundJobs;
using phase_1.BLL.Hubs;
using phase_1.Services;
using phase_1.Services.Identity;

namespace phase_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IOTPService, OTPService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IRoleManager, RoleManager>();
            builder.Services.AddScoped<ISignInManager, SignInManager>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IMatchRepository, MatchRepository>();
            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<INoShowStrikeRepository, NoShowStrikeRepository>();
            builder.Services.AddScoped<INoShowStrikeService, NoShowStrikeService>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
            builder.Services.AddScoped<IReminderService, ReminderService>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddScoped<ICaseRepository, CaseRepository>();
            builder.Services.AddScoped<ICaseService, CaseService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddHostedService<ReminderDispatcherHostedService>();
            builder.Services.AddHttpClient<ICaseAiAssistService, GeminiCaseAssistService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path;
                var isAuthPath = path.StartsWithSegments("/Auth", StringComparison.OrdinalIgnoreCase)
                    || path.StartsWithSegments("/api/auth", StringComparison.OrdinalIgnoreCase);
                var isApiPath = path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
                var isFrameworkPath = path.StartsWithSegments("/_framework", StringComparison.OrdinalIgnoreCase);
                var isErrorPath = path.StartsWithSegments("/Home/Error", StringComparison.OrdinalIgnoreCase);
                var isLoggedIn = context.Session.GetInt32("UserId").HasValue;

                if (!isLoggedIn && !isAuthPath && !isApiPath && !isFrameworkPath && !isErrorPath)
                {
                    context.Response.Redirect("/Auth/Register");
                    return;
                }

                await next();
            });

            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapControllerRoute(
                name: "patient-portal",
                pattern: "patient/{controller=Patients}/{action=Profile}/{id?}",
                defaults: new { portal = "patient" })
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "student-portal",
                pattern: "student/{controller=Student}/{action=Profile}/{id?}",
                defaults: new { portal = "student" })
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Register}/{id?}")
                .WithStaticAssets();
            

            app.Run();
        }
    }
}

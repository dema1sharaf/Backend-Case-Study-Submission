using EmailService.WebApi.Services;
using EmailService.WebApi.Settings;
using MassTransit;
using Microservice.TodoApp.Consumers;

namespace EmailService.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<TodoConsumer>(); //added this line
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                    cfg.ReceiveEndpoint("todoQueue", ep =>
                    {
                        ep.ConfigureConsumer<TodoConsumer>(provider); //changed
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddSingleton<IMailService, Services.MailService>();
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

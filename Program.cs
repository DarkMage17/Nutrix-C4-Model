using Structurizr;
using Structurizr.Api;

namespace fas_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator();
        }

        static void Generator()
        {
            const long workspaceId = 69571;
            const string apiKey = "20a089aa-1019-46e8-8b5e-d1b41c04a668";
            const string apiSecret = "b50b6ee7-6206-4e91-9170-b17ef8b63598";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Nutrix C4 Model - Sistema de Videollamadas", "Sistema de Videollamadas para asesorías virtuales con nutricionistas");
            Model model = workspace.Model;

            SoftwareSystem videocallSystem = model.AddSoftwareSystem("Nutrix Platform", "Permite las asesorías virtuales entre pacientes y nutricionistas");
            SoftwareSystem cnpAPI = model.AddSoftwareSystem("CNP API", "Permite la validación de nutricionistas");
            SoftwareSystem agoraAPI = model.AddSoftwareSystem("Agora API", "Permite las videollamadas para las asesorías virtuales");
            SoftwareSystem niubizAPI = model.AddSoftwareSystem("Niubiz API", "Permite manejar los cobros por asesorías");

            Person patient = model.AddPerson("Patient", "Usuario de la plataforma");
            Person nutritionist = model.AddPerson("Nutritionist", "Nutricionista registrado en la plataforma");

            patient.Uses(videocallSystem, "Usa");
            nutritionist.Uses(videocallSystem, "Usa");

            videocallSystem.Uses(agoraAPI, "Permite las videollamadas dentro de la plataforma");
            videocallSystem.Uses(cnpAPI, "Consulta información del nutricionista");
            videocallSystem.Uses(niubizAPI, "Permite los pagos dentro de la plataforma");

            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(videocallSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            videocallSystem.AddTags("SistemaVideollamadas");
            cnpAPI.AddTags("CNP_API");
            agoraAPI.AddTags("AgoraAPI");
            niubizAPI.AddTags("NiubizAPI");
            patient.AddTags("Patient");
            nutritionist.AddTags("Nutritionist");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Patient") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Nutritionist") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaVideollamadas") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("CNP_API") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("AgoraAPI") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("NiubizAPI") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container webApplication = videocallSystem.AddContainer("Web App", "Permite a los usuarios utilizar la plataforma para las asesorías", "Angular Web");
            Container landingPage = videocallSystem.AddContainer("Landing Page", "", "Angular Web");
            Container apiGateway = videocallSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");
            Container appointmentContext = videocallSystem.AddContainer("Appointment Context", "Bounded Context del Microservicio de Appointments", "Spring Boot port 8081");
            Container patientContext = videocallSystem.AddContainer("Patient Context", "Bounded Context del Microservicio de Pacientes", "Spring Boot port 8082");
            Container nutritionistContext = videocallSystem.AddContainer("Nutritionist Context", "Bounded Context del Microservicio de Nutricionistas", "Spring Boot port 8083");
            Container recipeContext = videocallSystem.AddContainer("Recipe Context", "Bounded Context del Microservicio de Recetas", "Spring Boot port 8084");
            Container dietContext = videocallSystem.AddContainer("Diet Context", "Bounded Context del Microservicio de Dietas", "Spring Boot port 8085");
            Container recommendationsContext = videocallSystem.AddContainer("Recommendations Context", "Bounded Context del Microservicio de Recomendaciones", "Spring Boot port 8086");
            Container specialtyContext = videocallSystem.AddContainer("Specialty Context", "Bounded Context del Microservicio de Especialidades", "Spring Boot port 8087");
            Container paymentContext = videocallSystem.AddContainer("Payment Context", "Bounded Context del Microservicio de Payments", "Spring Boot port 8088");
            Container professionalProfileContext = videocallSystem.AddContainer("Professional Profile Context", "Bounded Context del Microservicio de Professional Profiles", "Spring Boot port 8089");
            Container authenticationContext = videocallSystem.AddContainer("Authentication Context", "Bounded Context del Microservicio que gestiona la identidad de los usuarios", "Spring Boot port 8090");
            Container billingContext = videocallSystem.AddContainer("Billing Context", "Bounded Context del Microservicio de Billings", "Spring Boot port 8091");
            Container messageBus = videocallSystem.AddContainer("Bus de Mensajes en Cluster de Alta Disponibilidad", "Transporte de eventos del dominio.", "RabbitMQ");
            Container appointmentContextDatabase = videocallSystem.AddContainer("Appointment Context DB", "", "Oracle");
            Container patientContextDatabase = videocallSystem.AddContainer("Patient Context DB", "", "Oracle");
            Container nutritionistContextDatabase = videocallSystem.AddContainer("Nutritionist Context DB", "", "Oracle");
            Container recipeContextDatabase = videocallSystem.AddContainer("Recipe Context DB", "", "Oracle");
            Container dietContextDatabase = videocallSystem.AddContainer("Diet Context DB", "", "Oracle");
            Container recommendationsContextDatabase = videocallSystem.AddContainer("Recommendation Context DB", "", "Oracle");
            Container specialtyContextDatabase = videocallSystem.AddContainer("Specialty Context DB", "", "Oracle");
            Container paymentContextDatabase = videocallSystem.AddContainer("Payment Context DB", "", "Oracle");
            Container professionalProfileContextDatabase = videocallSystem.AddContainer("ProfessionalProfile Context DB", "", "Oracle");
            Container authenticationContextDatabase = videocallSystem.AddContainer("Authentica Context DB", "", "Oracle");
            Container billingContextDatabase = videocallSystem.AddContainer("Airport Context DB", "", "Oracle");

            patient.Uses(webApplication, "Consulta");
            patient.Uses(landingPage, "Consulta");

            nutritionist.Uses(webApplication, "Consulta");
            nutritionist.Uses(landingPage, "Consulta");

            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            apiGateway.Uses(appointmentContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(patientContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(nutritionistContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(recipeContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(dietContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(recommendationsContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(specialtyContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(paymentContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(professionalProfileContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(authenticationContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(billingContext, "API Request", "JSON/HTTPS");

            appointmentContext.Uses(messageBus, "Publica y consume eventos del dominio");
            appointmentContext.Uses(appointmentContextDatabase, "", "JDBC");
            appointmentContext.Uses(agoraAPI, "Permite las videollamadas dentro de la app");

            patientContext.Uses(messageBus, "Publica y consume eventos del dominio");
            patientContext.Uses(patientContextDatabase, "", "JDBC");

            nutritionistContext.Uses(messageBus, "Publica y consume eventos del dominio");
            nutritionistContext.Uses(nutritionistContextDatabase, "", "JDBC");
            nutritionistContext.Uses(cnpAPI, "Permite la validación de los datos del nutricionista");

            recipeContext.Uses(messageBus, "Publica y consume eventos del dominio");
            recipeContext.Uses(recipeContextDatabase, "", "JDBC");

            dietContext.Uses(messageBus, "Publica y consume eventos del dominio");
            dietContext.Uses(dietContextDatabase, "", "JDBC");

            recommendationsContext.Uses(messageBus, "Publica y consume eventos del dominio");
            recommendationsContext.Uses(recommendationsContextDatabase, "", "JDBC");

            specialtyContext.Uses(messageBus, "Publica y consume eventos del dominio");
            specialtyContext.Uses(specialtyContextDatabase, "", "JDBC");

            paymentContext.Uses(messageBus, "Publica y consume eventos del dominio");
            paymentContext.Uses(paymentContextDatabase, "", "JDBC");
            paymentContext.Uses(niubizAPI, "Permite los pagos por los servicios ofrecidos");

            professionalProfileContext.Uses(messageBus, "Publica y consume eventos del dominio");
            professionalProfileContext.Uses(professionalProfileContextDatabase, "", "JDBC");

            authenticationContext.Uses(messageBus, "Publica y consume eventos del dominio");
            authenticationContext.Uses(authenticationContextDatabase, "", "JDBC");

            billingContext.Uses(messageBus, "Publica y consume eventos del dominio");
            billingContext.Uses(billingContextDatabase, "", "JDBC");

            // Tags
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");

            appointmentContext.AddTags("AppointmentContext");
            appointmentContextDatabase.AddTags("AppointmentContextDatabase");

            patientContext.AddTags("PatientContext");
            patientContextDatabase.AddTags("PatientContextDatabase");

            nutritionistContext.AddTags("NutritionistContext");
            nutritionistContextDatabase.AddTags("NutritionistContextDatabase");

            recipeContext.AddTags("RecipeContext");
            recipeContextDatabase.AddTags("RecipeContextDatabase");

            dietContext.AddTags("DietContext");
            dietContextDatabase.AddTags("DietContextDatabase");

            recommendationsContext.AddTags("RecommendationContext");
            recommendationsContextDatabase.AddTags("RecommendationContextDatabase");

            specialtyContext.AddTags("SpecialtyContext");
            specialtyContextDatabase.AddTags("SpecialtyContextDatabase");

            paymentContext.AddTags("PaymentContext");
            paymentContextDatabase.AddTags("PaymentContextDatabase");

            professionalProfileContext.AddTags("ProfessionalProfileContext");
            professionalProfileContextDatabase.AddTags("ProfessionalProfileContextDatabase");

            authenticationContext.AddTags("AuthenticationContext");
            authenticationContextDatabase.AddTags("AuthenticationContextDatabase");

            billingContext.AddTags("BillingContext");
            billingContextDatabase.AddTags("BillingContextDatabase");

            messageBus.AddTags("MessageBus");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("AppointmentContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("PatientContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PatientContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("NutritionistContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NutritionistContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("RecipeContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecipeContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("DietContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DietContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("RecommendationContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecommendationContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("SpecialtyContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SpecialtyContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("PaymentContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("ProfessionalProfileContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProfessionalProfileContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("AuthenticationContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AuthenticationContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("BillingContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("BillingContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });


            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#fd8208", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(videocallSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.PaperSize = PaperSize.A3_Landscape;
            containerView.AddAllElements();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
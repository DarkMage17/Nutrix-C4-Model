using Structurizr;
using Structurizr.Api;

namespace nutrix_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator();
        }

        static void Generator()
        {
            const long workspaceId = 69868;
            const string apiKey = "39bb634b-5f5d-4363-9fdf-33aa689a4ce0";
            const string apiSecret = "8b4522e1-46ff-4ccd-aab8-5962a4764e7f";

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
            Container publicationsContext = videocallSystem.AddContainer("Publications Context", "Bounded Context del Microservicio de Publicaciones", "Spring Boot port 8084");
            Container messageBus = videocallSystem.AddContainer("Bus de Mensajes", "Transporte de eventos del dominio.", "Spring Cloud Bus");
            Container appointmentContextDatabase = videocallSystem.AddContainer("Appointment Context DB", "", "MySQL");
            Container patientContextDatabase = videocallSystem.AddContainer("Patient Context DB", "", "MySQL");
            Container nutritionistContextDatabase = videocallSystem.AddContainer("Nutritionist Context DB", "", "MySQL");
            Container publicationsContextDatabase = videocallSystem.AddContainer("Publications Context DB", "", "MySQL");
            Container eventContextDatabase = videocallSystem.AddContainer("Event Context DB", "", "MySQL");

            patient.Uses(webApplication, "Consulta");
            patient.Uses(landingPage, "Consulta");

            nutritionist.Uses(webApplication, "Consulta");
            nutritionist.Uses(landingPage, "Consulta");

            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            apiGateway.Uses(appointmentContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(patientContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(nutritionistContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(publicationsContext, "API Request", "JSON/HTTPS");

            appointmentContext.Uses(messageBus, "Publica y consume eventos del dominio");
            appointmentContext.Uses(appointmentContextDatabase, "", "JDBC");
            appointmentContext.Uses(eventContextDatabase, "", "JDBC");
            appointmentContext.Uses(agoraAPI, "Permite las videollamadas dentro de la app");

            patientContext.Uses(messageBus, "Publica y consume eventos del dominio");
            patientContext.Uses(niubizAPI, "Permite los pagos dentro de la app");
            patientContext.Uses(patientContextDatabase, "", "JDBC");
            patientContext.Uses(eventContextDatabase, "", "JDBC");

            nutritionistContext.Uses(messageBus, "Publica y consume eventos del dominio");
            nutritionistContext.Uses(nutritionistContextDatabase, "", "JDBC");
            nutritionistContext.Uses(eventContextDatabase, "", "JDBC");
            nutritionistContext.Uses(cnpAPI, "Permite la validación de los datos del nutricionista");

            publicationsContext.Uses(messageBus, "Publica y consume eventos del dominio");
            publicationsContext.Uses(publicationsContextDatabase, "", "JDBC");
            publicationsContext.Uses(eventContextDatabase, "", "JDBC");

            // Tags
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");
            eventContextDatabase.AddTags("EventContextDatabase");

            appointmentContext.AddTags("AppointmentContext");
            appointmentContextDatabase.AddTags("AppointmentContextDatabase");

            patientContext.AddTags("PatientContext");
            patientContextDatabase.AddTags("PatientContextDatabase");

            nutritionistContext.AddTags("NutritionistContext");
            nutritionistContextDatabase.AddTags("NutritionistContextDatabase");

            publicationsContext.AddTags("PublicationsContext");
            publicationsContextDatabase.AddTags("PublicationsContextDatabase");

            messageBus.AddTags("MessageBus");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("EventContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("AppointmentContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("PatientContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PatientContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("NutritionistContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NutritionistContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("PublicationsContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PublicationsContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });

            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#fd8208", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(videocallSystem, "Contenedor", "Diagrama de contenedores");
            containerView.AddAllElements();

            // 3. Diagrama de componentes patient

            ComponentView componentViewPatient = viewSet.CreateComponentView(patientContext, "ComponentsPatient", "Component Diagram");

            Component patientsCommandController = patientContext.AddComponent("Patients Command Controller", "REST API endpoints de patients", "Spring Boot REST Controller");
            Component patientsQueryController = patientContext.AddComponent("Patients Query Controller", "Permite a los pacientes acceder a su perfil y datos", "Spring Component");
            Component billsCommandController = patientContext.AddComponent("Bills Command Controller", "REST API endpoints de bills", "Spring Boot REST Controller");
            Component billsQueryController = patientContext.AddComponent("Bills Query Controller", "Permite acceder a los datos de bills", "Spring Component");
            Component paymentMethodsCommandController = patientContext.AddComponent("PaymentMethods Command Controller", "REST API endpoints de payment methods", "Spring Boot REST Controller");
            Component paymentMethodsQueryController = patientContext.AddComponent("PaymentMethods Query Controller", "Permite acceder a los datos de payment methods", "Spring Component");
            Component patientsApplicationService = patientContext.AddComponent("Patients Application Service","Provee métodos para los patients", "Spring Component");
            Component domainLayer = patientContext.AddComponent("Domain Layer","","Spring Component");
            Component patientRepository = patientContext.AddComponent("Patient Repository","Información de los patients", "Spring Component");
            Component billRepository = patientContext.AddComponent("Bill Repository", "Información de los bills", "Spring Component");
            Component paymentMethodRepository = patientContext.AddComponent("PaymentMethod Repository", "Información de los payment methods", "Spring Component");

            // Tags
            patientsCommandController.AddTags("PatientsCommandController");
            patientsQueryController.AddTags("PatientsQueryController");
            billsCommandController.AddTags("BillsCommandController");
            billsQueryController.AddTags("BillsQueryController");
            paymentMethodsCommandController.AddTags("PaymentMethodsCommandController");
            paymentMethodsQueryController.AddTags("PaymentMethodsQueryController");
            patientsApplicationService.AddTags("PatientsApplicationService");
            domainLayer.AddTags("DomainLayer");
            patientRepository.AddTags("PatientRepository");
            billRepository.AddTags("BillRepository");
            paymentMethodRepository.AddTags("PaymentMethodRepository");

            styles.Add(new ElementStyle("PatientsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PatientsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("BillsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("BillsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentMethodsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentMethodsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PatientsApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PatientRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("BillRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentMethodRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiGateway.Uses(patientsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(patientsQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(billsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(billsQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(paymentMethodsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(paymentMethodsQueryController, "", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            patientsCommandController.Uses(patientsApplicationService, "Invoca métodos de patients", "");
            patientsQueryController.Uses(patientsApplicationService, "Invoca métodos de patients", "");
            billsCommandController.Uses(patientsApplicationService, "Invoca métodos de bills", "");
            billsQueryController.Uses(patientsApplicationService, "Invoca métodos de bills", "");
            paymentMethodsCommandController.Uses(patientsApplicationService, "Invoca métodos de payment methods", "");
            paymentMethodsQueryController.Uses(patientsApplicationService, "Invoca métodos de payment methods", "");

            patientsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            patientsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            billsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            billsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            paymentMethodsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            paymentMethodsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");

            patientsApplicationService.Uses(domainLayer, "Usa", "");
            patientsApplicationService.Uses(patientRepository, "", "JDBC");
            patientsApplicationService.Uses(billRepository, "", "JDBC");
            patientsApplicationService.Uses(paymentMethodRepository, "", "JDBC");

            billRepository.Uses(niubizAPI, "", "JSON/HTTPS");
            patientRepository.Uses(patientContextDatabase, "", "JDBC");
            billRepository.Uses(patientContextDatabase, "", "JDBC");
            paymentMethodRepository.Uses(patientContextDatabase, "", "JDBC");

            componentViewPatient.Add(webApplication);
            componentViewPatient.Add(apiGateway);
            componentViewPatient.Add(patientContextDatabase);
            componentViewPatient.Add(eventContextDatabase);
            componentViewPatient.Add(niubizAPI);

            componentViewPatient.Add(patientsCommandController);
            componentViewPatient.Add(patientsQueryController);
            componentViewPatient.Add(billsCommandController);
            componentViewPatient.Add(billsQueryController);
            componentViewPatient.Add(paymentMethodsCommandController);
            componentViewPatient.Add(paymentMethodsQueryController);
            componentViewPatient.Add(patientsApplicationService);
            componentViewPatient.Add(patientRepository);
            componentViewPatient.Add(billRepository);
            componentViewPatient.Add(paymentMethodRepository);
            componentViewPatient.Add(domainLayer);

            //4. Diagrama de componentes nutritionist

            ComponentView componentViewNutritionist = viewSet.CreateComponentView(nutritionistContext, "ComponentsNutritionist", "Component Diagram");

            Component nutritionistsCommandController = nutritionistContext.AddComponent("Nutritionists Command Controller", "REST API endpoints de nutritionists", "Spring Boot REST Controller");
            Component nutritionistsQueryController = nutritionistContext.AddComponent("Nutritionists Query Controller", "Permite a los nutricionistas acceder a su perfil y datos", "Spring Component");
            Component professionalProfilesCommandController = nutritionistContext.AddComponent("Professional profiles Command Controller", "REST API endpoints de professional profiles", "Spring Boot REST Controller");
            Component professionalProfilesQueryController = nutritionistContext.AddComponent("Professional profiles Query Controller", "Permite acceder a los datos de professional profiles", "Spring Component");
            Component specialtiesCommandController = nutritionistContext.AddComponent("Specialties Command Controller", "REST API endpoints de specialties", "Spring Boot REST Controller");
            Component specialtiesQueryController = nutritionistContext.AddComponent("Specialties Query Controller", "Permite acceder a los datos de specialties", "Spring Component");
            Component nutritionistsApplicationService = nutritionistContext.AddComponent("Nutritionists Application Service", "Provee métodos para los nutritionists", "Spring Component");
            Component nutritionistRepository = nutritionistContext.AddComponent("Nutritionist Repository", "Información de los nutritionists", "Spring Component");
            Component professionalProfileRepository = nutritionistContext.AddComponent("Professional profile Repository", "Información de los professional profiles", "Spring Component");
            Component specialtyRepository = nutritionistContext.AddComponent("Specialty Repository", "Información de los specialties", "Spring Component");
            Component domainLayerNutritionist = nutritionistContext.AddComponent("Domain Layer", "", "Spring Component");

            // Tags
            nutritionistsCommandController.AddTags("NutritionistsCommandController");
            nutritionistsQueryController.AddTags("NutritionistsQueryController");
            professionalProfilesCommandController.AddTags("ProfessionalProfilesCommandController");
            professionalProfilesQueryController.AddTags("ProfessionalProfilesQueryController");
            specialtiesCommandController.AddTags("SpecialtiesCommandController");
            specialtiesQueryController.AddTags("SpecialtiesQueryController");
            nutritionistsApplicationService.AddTags("NutritionistsApplicationService");
            nutritionistRepository.AddTags("NutritionistRepository");
            professionalProfileRepository.AddTags("ProfessionalProfileRepository");
            specialtyRepository.AddTags("SpecialtyRepository");
            domainLayerNutritionist.AddTags("DomainLayerNutritionist");

            styles.Add(new ElementStyle("NutritionistsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NutritionistsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProfessionalProfilesCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProfessionalProfilesQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SpecialtiesCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SpecialtiesQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NutritionistsApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NutritionistRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProfessionalProfileRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SpecialtyRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DomainLayerNutritionist") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiGateway.Uses(nutritionistsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(nutritionistsQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(professionalProfilesCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(professionalProfilesQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(specialtiesCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(specialtiesQueryController, "", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            nutritionistsCommandController.Uses(nutritionistsApplicationService, "Invoca métodos de nutritionists", "");
            nutritionistsQueryController.Uses(nutritionistsApplicationService, "Invoca métodos de nutritionists", "");
            professionalProfilesCommandController.Uses(nutritionistsApplicationService, "Invoca métodos de professional profiles", "");
            professionalProfilesQueryController.Uses(nutritionistsApplicationService, "Invoca métodos de professional profiles", "");
            specialtiesCommandController.Uses(nutritionistsApplicationService, "Invoca métodos de specialties", "");
            specialtiesQueryController.Uses(nutritionistsApplicationService, "Invoca métodos de specialties", "");

            nutritionistsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            nutritionistsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            professionalProfilesCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            professionalProfilesQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            specialtiesCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            specialtiesQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");

            nutritionistsApplicationService.Uses(domainLayerNutritionist, "Usa", "");
            nutritionistsApplicationService.Uses(nutritionistRepository, "", "JDBC");
            nutritionistsApplicationService.Uses(professionalProfileRepository, "", "JDBC");
            nutritionistsApplicationService.Uses(specialtyRepository, "", "JDBC");

            nutritionistRepository.Uses(cnpAPI, "", "JSON/HTTPS");
            nutritionistRepository.Uses(nutritionistContextDatabase, "", "JDBC");
            professionalProfileRepository.Uses(nutritionistContextDatabase, "", "JDBC");
            specialtyRepository.Uses(nutritionistContextDatabase, "", "JDBC");

            componentViewNutritionist.Add(webApplication);
            componentViewNutritionist.Add(apiGateway);
            componentViewNutritionist.Add(nutritionistContextDatabase);
            componentViewNutritionist.Add(cnpAPI);
            componentViewNutritionist.Add(eventContextDatabase);

            componentViewNutritionist.Add(nutritionistsCommandController);
            componentViewNutritionist.Add(nutritionistsQueryController);
            componentViewNutritionist.Add(professionalProfilesCommandController);
            componentViewNutritionist.Add(professionalProfilesQueryController);
            componentViewNutritionist.Add(specialtiesCommandController);
            componentViewNutritionist.Add(specialtiesQueryController);
            componentViewNutritionist.Add(nutritionistsApplicationService);
            componentViewNutritionist.Add(nutritionistRepository);
            componentViewNutritionist.Add(professionalProfileRepository);
            componentViewNutritionist.Add(specialtyRepository);
            componentViewNutritionist.Add(domainLayerNutritionist);

            //5. Diagrama de componentes appointment

            ComponentView componentViewAppointment = viewSet.CreateComponentView(appointmentContext, "ComponentsAppointment", "Component Diagram");

            Component appointmentsCommandController = appointmentContext.AddComponent("Appointments Command Controller", "REST API endpoints de appointments", "Spring Boot REST Controller");
            Component appointmentsQueryController = appointmentContext.AddComponent("Appointments Query Controller", "Permite acceder a los datos de appointments", "Spring Component");
            Component dietsCommandController = appointmentContext.AddComponent("Diets Command Controller", "REST API endpoints de diets", "Spring Boot REST Controller");
            Component dietsQueryController = appointmentContext.AddComponent("Diets Query Controller", "Permite acceder a los datos de diets", "Spring Component");
            Component appointmentsApplicationService = appointmentContext.AddComponent("Appointments Application Service", "Provee métodos para los appointments", "Spring Component");
            Component appointmentRepository = appointmentContext.AddComponent("Appointment Repository", "Información de los appointments", "Spring Component");
            Component dietRepository = appointmentContext.AddComponent("Diet Repository", "Información de las diets", "Spring Component");
            Component domainLayerAppointment = appointmentContext.AddComponent("Domain Layer", "", "Spring Component");

            // Tags
            appointmentsCommandController.AddTags("AppointmentsCommandController");
            appointmentsQueryController.AddTags("AppointmentsQueryController");
            dietsCommandController.AddTags("DietsCommandController");
            dietsQueryController.AddTags("DietsQueryController");
            appointmentsApplicationService.AddTags("AppointmentsApplicationService");
            appointmentRepository.AddTags("AppointmentRepository");
            dietRepository.AddTags("DietRepository");
            domainLayerAppointment.AddTags("DomainLayerAppointment");

            styles.Add(new ElementStyle("AppointmentsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DietsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DietsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentsApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DietRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DomainLayerAppointment") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiGateway.Uses(appointmentsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(appointmentsQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(dietsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(dietsQueryController, "", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            appointmentsCommandController.Uses(appointmentsApplicationService, "Invoca métodos de appointments", "");
            appointmentsQueryController.Uses(appointmentsApplicationService, "Invoca métodos de appointments", "");
            dietsCommandController.Uses(appointmentsApplicationService, "Invoca métodos de diets", "");
            dietsQueryController.Uses(appointmentsApplicationService, "Invoca métodos de diets", "");

            appointmentsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            appointmentsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            dietsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            dietsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");

            appointmentsApplicationService.Uses(domainLayerAppointment, "Usa", "");
            appointmentsApplicationService.Uses(appointmentRepository, "", "JDBC");
            appointmentsApplicationService.Uses(dietRepository, "", "JDBC");

            appointmentRepository.Uses(agoraAPI, "", "JSON/HTTPS");
            appointmentRepository.Uses(appointmentContextDatabase, "", "JDBC");
            dietRepository.Uses(appointmentContextDatabase, "", "JDBC");

            componentViewAppointment.Add(webApplication);
            componentViewAppointment.Add(apiGateway);
            componentViewAppointment.Add(appointmentContextDatabase);
            componentViewAppointment.Add(agoraAPI);
            componentViewAppointment.Add(eventContextDatabase);

            componentViewAppointment.Add(appointmentsCommandController);
            componentViewAppointment.Add(appointmentsQueryController);
            componentViewAppointment.Add(dietsCommandController);
            componentViewAppointment.Add(dietsQueryController);
            componentViewAppointment.Add(appointmentsApplicationService);
            componentViewAppointment.Add(appointmentRepository);
            componentViewAppointment.Add(dietRepository);
            componentViewAppointment.Add(domainLayerAppointment);

            //6. Diagrama de componentes publications

            ComponentView componentViewPublications = viewSet.CreateComponentView(publicationsContext, "ComponentsPublication", "Component Diagram");

            Component recommendationsCommandController = publicationsContext.AddComponent("Recommendations Command Controller", "REST API endpoints de recommendations", "Spring Boot REST Controller");
            Component recommendationsQueryController = publicationsContext.AddComponent("Recommendations Query Controller", "Permite acceder a los datos de recommendations", "Spring Component");
            Component recipesCommandController = publicationsContext.AddComponent("Recipes Command Controller", "REST API endpoints de recipes", "Spring Boot REST Controller");
            Component recipesQueryController = publicationsContext.AddComponent("Recipes Query Controller", "Permite acceder a los datos de recipes", "Spring Component");
            Component publicationsApplicationService = publicationsContext.AddComponent("Publications Application Service", "Provee métodos para las publications", "Spring Component");
            Component recommendationRepository = publicationsContext.AddComponent("Recommendation Repository", "Información de las recommendations", "Spring Component");
            Component recipeRepository = publicationsContext.AddComponent("Recipe Repository", "Información de las recipes", "Spring Component");
            Component domainLayerPublication = publicationsContext.AddComponent("Domain Layer", "", "Spring Component");

            // Tags
            recommendationsCommandController.AddTags("RecommendationsCommandController");
            recommendationsQueryController.AddTags("RecommendationsQueryController");
            recipesCommandController.AddTags("RecipesCommandController");
            recipesQueryController.AddTags("RecipesQueryController");
            publicationsApplicationService.AddTags("PublicationsApplicationService");
            recommendationRepository.AddTags("RecommendationRepository");
            recipeRepository.AddTags("RecipeRepository");
            domainLayerPublication.AddTags("DomainLayerPublication");

            styles.Add(new ElementStyle("RecommendationsCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecommendationsQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecipesCommandController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecipesQueryController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PublicationsApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecommendationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecipeRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DomainLayerPublication") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiGateway.Uses(recommendationsCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(recommendationsQueryController, "", "JSON/HTTPS");
            apiGateway.Uses(recipesCommandController, "", "JSON/HTTPS");
            apiGateway.Uses(recipesQueryController, "", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            recommendationsCommandController.Uses(publicationsApplicationService, "Invoca métodos de recommendations", "");
            recommendationsQueryController.Uses(publicationsApplicationService, "Invoca métodos de recommendations", "");
            recipesCommandController.Uses(publicationsApplicationService, "Invoca métodos de recipes", "");
            recipesQueryController.Uses(publicationsApplicationService, "Invoca métodos de recipes", "");

            recommendationsCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            recommendationsQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");
            recipesCommandController.Uses(eventContextDatabase, "Escribe en la DB", "");
            recipesQueryController.Uses(eventContextDatabase, "Escribe en la DB", "");

            publicationsApplicationService.Uses(domainLayerPublication, "Usa", "");
            publicationsApplicationService.Uses(recommendationRepository, "", "JDBC");
            publicationsApplicationService.Uses(recipeRepository, "", "JDBC");

            recommendationRepository.Uses(publicationsContextDatabase, "", "JDBC");
            recipeRepository.Uses(publicationsContextDatabase, "", "JDBC");

            componentViewPublications.Add(webApplication);
            componentViewPublications.Add(apiGateway);
            componentViewPublications.Add(publicationsContextDatabase);
            componentViewPublications.Add(eventContextDatabase);

            componentViewPublications.Add(recommendationsCommandController);
            componentViewPublications.Add(recommendationsQueryController);
            componentViewPublications.Add(recipesCommandController);
            componentViewPublications.Add(recipesQueryController);
            componentViewPublications.Add(publicationsApplicationService);
            componentViewPublications.Add(recommendationRepository);
            componentViewPublications.Add(recipeRepository);
            componentViewPublications.Add(domainLayerPublication);

            // Configuraciones de la vista
            contextView.PaperSize = PaperSize.A5_Landscape;
            containerView.PaperSize = PaperSize.A3_Landscape;
            componentViewPatient.PaperSize = PaperSize.A4_Landscape;
            componentViewNutritionist.PaperSize = PaperSize.A4_Landscape;
            componentViewAppointment.PaperSize = PaperSize.A4_Landscape;
            componentViewPublications.PaperSize = PaperSize.A4_Landscape;

            // Actualizar Workspace
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
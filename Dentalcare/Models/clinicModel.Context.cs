﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dentalcare.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class clinicEntities : DbContext
    {
        public clinicEntities()
            : base("name=clinicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Assisstant> Assisstants { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Bill_Service> Bill_Service { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ConsumableMaterial> ConsumableMaterials { get; set; }
        public virtual DbSet<Dentist> Dentists { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FixedMaterial> FixedMaterials { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Ingredient_ConsumableMaterial> Ingredient_ConsumableMaterial { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Material_Category> Material_Category { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Prescription_Medicine> Prescription_Medicine { get; set; }
        public virtual DbSet<Receptionist> Receptionists { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Service_Category> Service_Category { get; set; }
        public virtual DbSet<NEWS> NEWS { get; set; }
        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<Footer> Footers { get; set; }
    
        public virtual int procAddAccountAndPerson(string username, string password, string name, string phoneNumber, string email, Nullable<int> salary, string address, Nullable<bool> gender, Nullable<System.DateTime> birthday, string nation, Nullable<int> role, string img, string falID, string title, string metaAccount, string metaPerson, string metaPersonDetail)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("phoneNumber", phoneNumber) :
                new ObjectParameter("phoneNumber", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var salaryParameter = salary.HasValue ?
                new ObjectParameter("salary", salary) :
                new ObjectParameter("salary", typeof(int));
    
            var addressParameter = address != null ?
                new ObjectParameter("address", address) :
                new ObjectParameter("address", typeof(string));
    
            var genderParameter = gender.HasValue ?
                new ObjectParameter("gender", gender) :
                new ObjectParameter("gender", typeof(bool));
    
            var birthdayParameter = birthday.HasValue ?
                new ObjectParameter("birthday", birthday) :
                new ObjectParameter("birthday", typeof(System.DateTime));
    
            var nationParameter = nation != null ?
                new ObjectParameter("nation", nation) :
                new ObjectParameter("nation", typeof(string));
    
            var roleParameter = role.HasValue ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(int));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            var falIDParameter = falID != null ?
                new ObjectParameter("falID", falID) :
                new ObjectParameter("falID", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var metaAccountParameter = metaAccount != null ?
                new ObjectParameter("MetaAccount", metaAccount) :
                new ObjectParameter("MetaAccount", typeof(string));
    
            var metaPersonParameter = metaPerson != null ?
                new ObjectParameter("MetaPerson", metaPerson) :
                new ObjectParameter("MetaPerson", typeof(string));
    
            var metaPersonDetailParameter = metaPersonDetail != null ?
                new ObjectParameter("MetaPersonDetail", metaPersonDetail) :
                new ObjectParameter("MetaPersonDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddAccountAndPerson", usernameParameter, passwordParameter, nameParameter, phoneNumberParameter, emailParameter, salaryParameter, addressParameter, genderParameter, birthdayParameter, nationParameter, roleParameter, imgParameter, falIDParameter, titleParameter, metaAccountParameter, metaPersonParameter, metaPersonDetailParameter);
        }
    
        public virtual int procAddAdvertisement(string title, string msg, string img, string meta)
        {
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var msgParameter = msg != null ?
                new ObjectParameter("msg", msg) :
                new ObjectParameter("msg", typeof(string));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddAdvertisement", titleParameter, msgParameter, imgParameter, metaParameter);
        }
    
        public virtual int procAddAppointment(string denID, string patID, Nullable<System.DateTime> timeStart, Nullable<System.DateTime> timeEnd, string symptom, string state, string note, string meta)
        {
            var denIDParameter = denID != null ?
                new ObjectParameter("denID", denID) :
                new ObjectParameter("denID", typeof(string));
    
            var patIDParameter = patID != null ?
                new ObjectParameter("patID", patID) :
                new ObjectParameter("patID", typeof(string));
    
            var timeStartParameter = timeStart.HasValue ?
                new ObjectParameter("timeStart", timeStart) :
                new ObjectParameter("timeStart", typeof(System.DateTime));
    
            var timeEndParameter = timeEnd.HasValue ?
                new ObjectParameter("timeEnd", timeEnd) :
                new ObjectParameter("timeEnd", typeof(System.DateTime));
    
            var symptomParameter = symptom != null ?
                new ObjectParameter("symptom", symptom) :
                new ObjectParameter("symptom", typeof(string));
    
            var stateParameter = state != null ?
                new ObjectParameter("state", state) :
                new ObjectParameter("state", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("note", note) :
                new ObjectParameter("note", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddAppointment", denIDParameter, patIDParameter, timeStartParameter, timeEndParameter, symptomParameter, stateParameter, noteParameter, metaParameter);
        }
    
        public virtual int procAddBill(string patId, string meta)
        {
            var patIdParameter = patId != null ?
                new ObjectParameter("patId", patId) :
                new ObjectParameter("patId", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddBill", patIdParameter, metaParameter);
        }
    
        public virtual int procAddBill_Service(string billId, string serId, Nullable<int> quantity, string meta)
        {
            var billIdParameter = billId != null ?
                new ObjectParameter("billId", billId) :
                new ObjectParameter("billId", typeof(string));
    
            var serIdParameter = serId != null ?
                new ObjectParameter("serId", serId) :
                new ObjectParameter("serId", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddBill_Service", billIdParameter, serIdParameter, quantityParameter, metaParameter);
        }
    
        public virtual int procAddCalendar(Nullable<System.DateTime> timeStart, Nullable<System.DateTime> timeEnd, string personID, string meta)
        {
            var timeStartParameter = timeStart.HasValue ?
                new ObjectParameter("timeStart", timeStart) :
                new ObjectParameter("timeStart", typeof(System.DateTime));
    
            var timeEndParameter = timeEnd.HasValue ?
                new ObjectParameter("timeEnd", timeEnd) :
                new ObjectParameter("timeEnd", typeof(System.DateTime));
    
            var personIDParameter = personID != null ?
                new ObjectParameter("personID", personID) :
                new ObjectParameter("personID", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddCalendar", timeStartParameter, timeEndParameter, personIDParameter, metaParameter);
        }
    
        public virtual int procAddClinic(string name, string phoneNumber, string address, string img, string email, string facebook, string zalo, string instagram, string youtube, string title, string msg, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("phoneNumber", phoneNumber) :
                new ObjectParameter("phoneNumber", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("address", address) :
                new ObjectParameter("address", typeof(string));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var facebookParameter = facebook != null ?
                new ObjectParameter("facebook", facebook) :
                new ObjectParameter("facebook", typeof(string));
    
            var zaloParameter = zalo != null ?
                new ObjectParameter("zalo", zalo) :
                new ObjectParameter("zalo", typeof(string));
    
            var instagramParameter = instagram != null ?
                new ObjectParameter("instagram", instagram) :
                new ObjectParameter("instagram", typeof(string));
    
            var youtubeParameter = youtube != null ?
                new ObjectParameter("youtube", youtube) :
                new ObjectParameter("youtube", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var msgParameter = msg != null ?
                new ObjectParameter("msg", msg) :
                new ObjectParameter("msg", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddClinic", nameParameter, phoneNumberParameter, addressParameter, imgParameter, emailParameter, facebookParameter, zaloParameter, instagramParameter, youtubeParameter, titleParameter, msgParameter, metaParameter);
        }
    
        public virtual int procAddComment(string patID, string title, string msg, string img, string meta)
        {
            var patIDParameter = patID != null ?
                new ObjectParameter("patID", patID) :
                new ObjectParameter("patID", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var msgParameter = msg != null ?
                new ObjectParameter("msg", msg) :
                new ObjectParameter("msg", typeof(string));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddComment", patIDParameter, titleParameter, msgParameter, imgParameter, metaParameter);
        }
    
        public virtual int procAddConsumableMaterial(Nullable<System.DateTime> expDate, string meta, string id)
        {
            var expDateParameter = expDate.HasValue ?
                new ObjectParameter("expDate", expDate) :
                new ObjectParameter("expDate", typeof(System.DateTime));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddConsumableMaterial", expDateParameter, metaParameter, idParameter);
        }
    
        public virtual int procAddFaculty(string name, string descrip, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var descripParameter = descrip != null ?
                new ObjectParameter("descrip", descrip) :
                new ObjectParameter("descrip", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddFaculty", nameParameter, descripParameter, metaParameter);
        }
    
        public virtual int procAddFixedMaterial(string meta, string id)
        {
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddFixedMaterial", metaParameter, idParameter);
        }
    
        public virtual int procAddIngredient(string name, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddIngredient", nameParameter, metaParameter);
        }
    
        public virtual int procAddIngredient_ConsumableMaterial(string ingreID, string consumID)
        {
            var ingreIDParameter = ingreID != null ?
                new ObjectParameter("ingreID", ingreID) :
                new ObjectParameter("ingreID", typeof(string));
    
            var consumIDParameter = consumID != null ?
                new ObjectParameter("consumID", consumID) :
                new ObjectParameter("consumID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddIngredient_ConsumableMaterial", ingreIDParameter, consumIDParameter);
        }
    
        public virtual int procAddMaterial(string name, string cateId, string calUnit, Nullable<int> quantity, string func, Nullable<System.DateTime> mfgDate, string meta, string img)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var cateIdParameter = cateId != null ?
                new ObjectParameter("cateId", cateId) :
                new ObjectParameter("cateId", typeof(string));
    
            var calUnitParameter = calUnit != null ?
                new ObjectParameter("calUnit", calUnit) :
                new ObjectParameter("calUnit", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            var funcParameter = func != null ?
                new ObjectParameter("func", func) :
                new ObjectParameter("func", typeof(string));
    
            var mfgDateParameter = mfgDate.HasValue ?
                new ObjectParameter("mfgDate", mfgDate) :
                new ObjectParameter("mfgDate", typeof(System.DateTime));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddMaterial", nameParameter, cateIdParameter, calUnitParameter, quantityParameter, funcParameter, mfgDateParameter, metaParameter, imgParameter);
        }
    
        public virtual int procAddMaterial_Category(string name, string descrip, string note, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var descripParameter = descrip != null ?
                new ObjectParameter("descrip", descrip) :
                new ObjectParameter("descrip", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("note", note) :
                new ObjectParameter("note", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddMaterial_Category", nameParameter, descripParameter, noteParameter, metaParameter);
        }
    
        public virtual int procAddMedicine(string id, string ins, Nullable<int> price, string caredActor, string meta)
        {
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            var insParameter = ins != null ?
                new ObjectParameter("ins", ins) :
                new ObjectParameter("ins", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("price", price) :
                new ObjectParameter("price", typeof(int));
    
            var caredActorParameter = caredActor != null ?
                new ObjectParameter("caredActor", caredActor) :
                new ObjectParameter("caredActor", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddMedicine", idParameter, insParameter, priceParameter, caredActorParameter, metaParameter);
        }
    
        public virtual int procAddMenu(string name, string link, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var linkParameter = link != null ?
                new ObjectParameter("link", link) :
                new ObjectParameter("link", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddMenu", nameParameter, linkParameter, metaParameter);
        }
    
        public virtual int procAddNews(string title, string msg, string meta)
        {
            var titleParameter = title != null ?
                new ObjectParameter("title", title) :
                new ObjectParameter("title", typeof(string));
    
            var msgParameter = msg != null ?
                new ObjectParameter("msg", msg) :
                new ObjectParameter("msg", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddNews", titleParameter, msgParameter, metaParameter);
        }
    
        public virtual int procAddPrescription(string denId, string patId, string billId, string note, string meta)
        {
            var denIdParameter = denId != null ?
                new ObjectParameter("denId", denId) :
                new ObjectParameter("denId", typeof(string));
    
            var patIdParameter = patId != null ?
                new ObjectParameter("patId", patId) :
                new ObjectParameter("patId", typeof(string));
    
            var billIdParameter = billId != null ?
                new ObjectParameter("billId", billId) :
                new ObjectParameter("billId", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("note", note) :
                new ObjectParameter("note", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddPrescription", denIdParameter, patIdParameter, billIdParameter, noteParameter, metaParameter);
        }
    
        public virtual int procAddPrescription_Medicine(string denId, string patId, string billId, string medID, Nullable<int> quantityMedicine, string meta)
        {
            var denIdParameter = denId != null ?
                new ObjectParameter("denId", denId) :
                new ObjectParameter("denId", typeof(string));
    
            var patIdParameter = patId != null ?
                new ObjectParameter("patId", patId) :
                new ObjectParameter("patId", typeof(string));
    
            var billIdParameter = billId != null ?
                new ObjectParameter("billId", billId) :
                new ObjectParameter("billId", typeof(string));
    
            var medIDParameter = medID != null ?
                new ObjectParameter("medID", medID) :
                new ObjectParameter("medID", typeof(string));
    
            var quantityMedicineParameter = quantityMedicine.HasValue ?
                new ObjectParameter("quantityMedicine", quantityMedicine) :
                new ObjectParameter("quantityMedicine", typeof(int));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddPrescription_Medicine", denIdParameter, patIdParameter, billIdParameter, medIDParameter, quantityMedicineParameter, metaParameter);
        }
    
        public virtual int procAddService(string name, Nullable<int> price, string calUnit, string note, string meta, string img, string cateId, string descrip, string caredActor)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("price", price) :
                new ObjectParameter("price", typeof(int));
    
            var calUnitParameter = calUnit != null ?
                new ObjectParameter("calUnit", calUnit) :
                new ObjectParameter("calUnit", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("note", note) :
                new ObjectParameter("note", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            var imgParameter = img != null ?
                new ObjectParameter("img", img) :
                new ObjectParameter("img", typeof(string));
    
            var cateIdParameter = cateId != null ?
                new ObjectParameter("cateId", cateId) :
                new ObjectParameter("cateId", typeof(string));
    
            var descripParameter = descrip != null ?
                new ObjectParameter("descrip", descrip) :
                new ObjectParameter("descrip", typeof(string));
    
            var caredActorParameter = caredActor != null ?
                new ObjectParameter("caredActor", caredActor) :
                new ObjectParameter("caredActor", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddService", nameParameter, priceParameter, calUnitParameter, noteParameter, metaParameter, imgParameter, cateIdParameter, descripParameter, caredActorParameter);
        }
    
        public virtual int procAddService_Category(string name, string descip, string note, string meta)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var descipParameter = descip != null ?
                new ObjectParameter("descip", descip) :
                new ObjectParameter("descip", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("note", note) :
                new ObjectParameter("note", typeof(string));
    
            var metaParameter = meta != null ?
                new ObjectParameter("meta", meta) :
                new ObjectParameter("meta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddService_Category", nameParameter, descipParameter, noteParameter, metaParameter);
        }
    
        public virtual int procAddAvatar(string personId)
        {
            var personIdParameter = personId != null ?
                new ObjectParameter("personId", personId) :
                new ObjectParameter("personId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddAvatar", personIdParameter);
        }
    }
}

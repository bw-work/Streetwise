using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AutomationCore.utility;

namespace Streetwise.Utility
{
    class DBUtility : AutomationCore.utility.DBAccess
    {

        public DBUtility()
        {
            dbDatabase = "HPG_IdeaManagementSBX01";
            dbServer = "10.248.18.35";
            dbUsername = "xpanxion";
            dbPassword = "xpanxion";
        }

        public enum IdeaStatus : short
        {
            Declined = 1,
            Submitted = 2,
            UnderReview = 3
        }

        public enum WorkflowStep : short
        {
            Streetwise = 0,
            User = 1,
            Boss = 2,
            SME = 3,
            Admin = 4,
            Public = 5
        }

        public enum QualifiedIdeaStatus : short
        {
            NotApplicable = 1,
            NotReadyToPublish = 2,
            ReadyToPublish = 3,
            Retired = 4
        }
        public enum PublishedTypeStatus : short
        {
            Accepted = 1,
            Published = 2,
            Suspended = 3,
            Retired = 4,
            Duplicate = 5,
            Nonviewable = 6,
            Streetwise = 7
        }

        public enum MembershipType : short
        {
            HCA = 1,
            GPO = 2,
            NonGPO = 3
        }

        /// <summary>
        /// Sets specified user's IsDcrd, IsSme, IsAdmin properties in the Database
        /// </summary>
        /// <param name="username">Full username (domain\username)</param>
        /// <param name="IsDcrd">Is user a DCRD</param>
        /// <param name="IsSme">Is user a SME</param>
        /// <param name="IsAdmin">Is user an Admin</param>
        public void SetUser(string username, bool IsDcrd = false, bool IsSme = false, bool IsAdmin = false)
        {
            string statement =
                string.Format("UPDATE Users SET IsDcrd = '{0}', IsSme = '{1}', IsAdmin = '{2}' WHERE Username = '{3}';",
                              IsDcrd, IsSme, IsAdmin, username);
            ExecuteStatement(statement);
        }

        /// <summary>
        /// Inserts inplementation for a qualified idea for a facility
        /// </summary>
        /// <param name="QualifiedIdea">Integer, qualified idea number</param>
        /// <param name="COID">string, Facility COID</param>
        /// <param name="ImplementDate">datatime, year/month to add implementation for</param>
        /// <param name="Status">NOT IN USE AT THE MOMENT - DEFAULTS TO IMPLEMENTED</param>
        /// <param name="User">domain\3-4 of user making change</param>
        public void ImplementQualifiedIdea(int QualifiedIdea, string COID, DateTime ImplementDate, Enums.ImplementedStatus Status = Enums.ImplementedStatus.Implemented, string User = "")
        {
            if (String.IsNullOrEmpty(User))
            {
                User = AutomationCore.SuperTest.Controller.fields["Domain"] + "\\" + AutomationCore.SuperTest.Controller.fields["UserId"];
            }
            string statement =
                string.Format(
                    "INSERT INTO [QualifiedIdeaImplementations] (QualifiedIdeaId, FacilityId, QualifiedIdeaImplementationStatus, Comment, CreatedDate, CreatedBy, ImplementationDate) VALUES ({0}, (SELECT FacilityId FROM Facilities WHERE CoId = '{1}'), 3, 'Implemented via script', GETDATE(), UPPER('{2}'), '{3}-01');", QualifiedIdea.ToString(), COID, User, ImplementDate.ToString("yyyy-MM"));
            ExecuteStatement(statement);
        }

        public void StreetwiseImportAddPrefix(int IdeaId, string TitlePrefix = "", string DescriptionPrefix = "")
        {
            string statement =
                string.Format(
                    "UPDATE [StreetwiseImportedIdeas] SET PublishedTitleText = '{1}' + PublishedTitleText, PublishedDescription = '{2}' + PublishedDescription WHERE IdeaId = {0}",
                    IdeaId.ToString(), TitlePrefix, DescriptionPrefix);
            ExecuteStatement(statement);
        }

        public void StreetwiseImportUpdateIdea(int IdeaId, string Prefix)
        {
            string statement =
                string.Format(
                    "UPDATE [StreetwiseImportedIdeas] SET PublishedTitleText = '{1}' + PublishedTitleText, PublishedDescription = '{1}' + PublishedDescription, IsUpdated = 'TRUE' WHERE IdeaId = {0}",
                    IdeaId.ToString(), Prefix);
            ExecuteStatement(statement);
        }

        public int DeleteResultType(string ResultTypeName)
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Deleting ResultType named '" + ResultTypeName + "'...");
            return ExecuteStatement(string.Format("DELETE FROM ResultTypes WHERE Name = '{0}';", ResultTypeName));
        }

        /// <summary>
        /// Safe way to update a PageModule body
        /// </summary>
        /// <param name="body">HTML for the PageModule</param>
        /// <param name="moduleId">ID of the PageModule</param>
        /// <returns></returns>
        //public string UpdatePageModule(string body, string moduleId)
        //{
        //    try
        //    {
        //        using(SqlCommand cmd = SQLConnection.CreateCommand())
        //        {
        //            cmd.CommandText = "UPDATE [PageModules] SET [Body] = (@body) WHERE [PageModuleId] = (@moduleId)";
        //            cmd.Parameters.Add("@body", body);
        //            cmd.Parameters.Add("@moduleId", moduleId);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return "ERROR ON PAGEMODULE UPDATE: " + e.Message + "\r\n" + e.StackTrace;
        //    }
        //    return "PageModule " + moduleId + " updated!";
        //}

        /// <summary>
        /// Deletes all answers to questions for a single Idea
        /// </summary>
        /// <param name="IdeaId">ID of the Idea</param>
        /// <returns>int number of how many records were deleted</returns>
        public int DeleteAnswersForIdea(int IdeaId)
        {
            return ExecuteStatement(string.Format("DELETE FROM [IdeaQuestionResponses] WHERE IdeaId = {0}", IdeaId.ToString()));
        }

        /// <summary>
        /// Deletes specified answers to questions for a single Idea
        /// </summary>
        /// <param name="IdeaId">ID of the Idea</param>
        /// <param name="QuestionIds">int array of questions to delete answers for</param>
        /// <returns>int number of how many records were deleted</returns>
        public int DeleteAnswersForIdea(int IdeaId, int[] QuestionIds)
        {
            return ExecuteStatement(string.Format("DELETE FROM [IdeaQuestionResponses] WHERE IdeaId = {0} AND IdeaQuestionId IN ({1})", IdeaId.ToString(), string.Join(",", QuestionIds)));
        }

        /// <summary>
        /// Deletes a question matching supplied string (and optionally all answers as well)
        /// </summary>
        /// <param name="question">exact match of the question</param>
        /// <param name="DeleteAnswers">true=delete questions as well, false=don't delete any questions</param>
        /// <returns>int number of how many questions were deleted</returns>
        public int DeleteIdeaQuestion(string question, bool DeleteAnswers = true)
        {
            if(DeleteAnswers) ExecuteStatement(
                string.Format(
                    "DELETE FROM [IdeaQuestionResponses] WHERE IdeaQuestionID = (SELECT IdeaQuestionID FROM [IdeaQuestions] WHERE Question = '{0}')",
                    question));
            return ExecuteStatement(string.Format("DELETE FROM [IdeaQuestions] WHERE Question = '{0}'", question));
        }

        /// <summary>
        /// Delete a Department with no associated Ideas
        /// </summary>
        /// <param name="nameLike">Name of Department LIKE in SQL form (example %TC1234%)</param>
        /// <returns></returns>
        public int DeleteDepartmentByName(string nameLike)
        {
            int rv = ExecuteStatement("DELETE FROM Departments WHERE Name LIKE '"
                + nameLike
                + "' AND DepartmentId IN (SELECT Departments.DepartmentId FROM Departments LEFT JOIN QualifiedIdeas ON Departments.DepartmentId = QualifiedIdeas.DepartmentId GROUP BY Departments.DepartmentId HAVING COUNT(QualifiedIdeas.IdeaId) <= 0);");
            AutomationCore.base_tests.BaseTest.WriteInfoReport("Deleting Departments matching '" + nameLike + "' result: " + rv.ToString());
            return rv;
        }

        /// <summary>
        /// Delete a Category with no associated Ideas
        /// </summary>
        /// <param name="nameLike">Name of Category LIKE in SQL form (example %TC1234%)</param>
        /// <returns></returns>
        public int DeleteCategoryByName(string nameLike)
        {
            int rv =
                ExecuteStatement(
                    "DELETE FROM Categories WHERE Name LIKE '"
                    + nameLike
                    + "' AND CategoryId IN (SELECT Categories.CategoryId FROM Categories LEFT JOIN PublishedIdeas ON Categories.CategoryId = PublishedIdeas.CategoryId GROUP BY Categories.CategoryId HAVING COUNT(PublishedIdeas.PublishedIdeasId) <= 0);");
            AutomationCore.base_tests.BaseTest.WriteInfoReport("Deleting Categories matching '" + nameLike + "' result: " + rv.ToString());
            return rv;
        }

        /// <summary>
        /// Deletes an idea directly from the Database
        /// </summary>
        /// <param name="ideaNumber">Integer: IdeaNumber</param>
        /// <param name="reqPublished">Require Published Idea for Delete?  False to delete idea regardless of Published State</param>
        /// <returns></returns>
        public int DeleteIdeaByIdeaNumber(int ideaNumber, bool reqPublished = false)
        {
            if (ExecuteStatement("DELETE FROM QualifiedIdeas WHERE IdeaId = " + ideaNumber) <= 0 && reqPublished)
            {
                return -1;
            }
            ExecuteStatement("DELETE FROM IdeaComments WHERE IdeaId = " + ideaNumber);
            ExecuteStatement("DELETE FROM IdeaContacts WHERE IdeaId = " + ideaNumber);
            return ExecuteStatement("DELETE FROM Ideas WHERE IdeaId = " + ideaNumber);
        }

        public DataTable GetIdeasByName(string nameLike)
        {
            DataTable returnTable = new DataTable("Ideas");
            returnTable.Load(
                GetRecords(
                "SELECT Ideas.*, PublishedIdeas.* FROM Ideas LEFT JOIN PublishedIdeas ON PublishedIdeas.IdeaId = Ideas.IdeaId WHERE Ideas.IdeaName LIKE '"
                + nameLike + "';"));
            return returnTable;
        }

        public DataTable GetIdeasByNumber(int ideaNumber)
        {
            DataTable returnTable = new DataTable("Ideas");
            returnTable.Load(
                GetRecords(
                "SELECT Ideas.*, PublishedIdeas.* FROM Ideas LEFT JOIN PublishedIdeas ON PublishedIdeas.IdeaId = Ideas.IdeaId WHERE Ideas.IdeaID = '"
                + ideaNumber + "';"));
            return returnTable;
        }

        public DataTable GetPublishedIdeas()
        {
            DataTable returnTable = new DataTable("Ideas");
            returnTable.Load(GetRecords("SELECT [qi].[IdeaId], [qi].[Title], [qi].[Description], [qi].[ImpactLevel], [qi].[EffortLevel], [qi].[PublishedDate], [qi].[UpdatedDate], [cat].[Name] as Category ,[dep].[Name] as Department FROM [HPG_IdeaManagementSBX].[dbo].[QualifiedIdeas] as qi LEFT JOIN [HPG_IdeaManagementSBX].[dbo].[Categories] as cat ON qi.CategoryId = cat.CategoryId LEFT JOIN [HPG_IdeaManagementSBX].[dbo].Departments as dep ON qi.DepartmentId = dep.DepartmentId WHERE qi.MembershipTypeId = 2 AND UpdatedDate IS NOT NULL AND StatusId = 2"));
            return returnTable;
        }

        public DataTable GetPublishedIdeas(string qWhere)
        {
            DataTable returnTable = new DataTable("Ideas");
            returnTable.Load(GetRecords("SELECT [qi].[IdeaId], [qi].[Title], [qi].[Description], [qi].[ImpactLevel], [qi].[EffortLevel], [qi].[PublishedDate], [qi].[UpdatedDate], [cat].[Name] as Category ,[dep].[Name] as Department FROM [HPG_IdeaManagementSBX].[dbo].[QualifiedIdeas] as qi LEFT JOIN [HPG_IdeaManagementSBX].[dbo].[Categories] as cat ON qi.CategoryId = cat.CategoryId LEFT JOIN [HPG_IdeaManagementSBX].[dbo].Departments as dep ON qi.DepartmentId = dep.DepartmentId WHERE qi.MembershipTypeId = 2 AND UpdatedDate IS NOT NULL AND StatusId = 2 AND " + qWhere + ";"));
            return returnTable;
        }

        public void SoftDeleteIdeas(int[] IdeaIDs, bool validate = true)
        {
            if (IdeaIDs.Any())
            {
                int raff = ExecuteStatement("UPDATE Ideas SET Deleted = 'TRUE' WHERE IdeaId IN (" + string.Join(",", IdeaIDs) + ");");
                if(validate) HpgAssert.AreEqual(IdeaIDs.Count().ToString(), raff.ToString(), "Verify all records were marked as deleted.");
            }
        }

        public int CreateNewIdea(string ideaName, string ideaDescription, IdeaStatus ideaStatus = IdeaStatus.UnderReview, string userID = "",
            string contactName = "Automation", string contactPhone = "515-555-1212", string contactEmail = "hpg.automation@hcahealthcare.com",
            WorkflowStep workflowStep = WorkflowStep.User, string assignedTo="")
        {
            if (String.IsNullOrEmpty(userID))
            {
                userID = AutomationCore.SuperTest.Controller.fields["Domain"] + "\\" + AutomationCore.SuperTest.Controller.fields["UserId"];
            }
            if (string.IsNullOrEmpty(assignedTo))
            {
                assignedTo = userID;
            }
            DataTable newIdea = new DataTable("Ideas");
            string insertStatememt = string.Format(@"INSERT INTO Ideas ([Title], [Description], [StatusId], [CreatedDate], [UpdatedDate], [SubmittedDate], [CreatedBy], [UpdatedBy], [SubmittedBy], [AssignTo], [WorkflowStep]) VALUES ('{0}', '{1}', {3}, GETDATE(), GETDATE(), GETDATE(), '{2}', '{2}', '{2}', '{5}', {4}); SELECT SCOPE_IDENTITY() as NewIdeaNumber;", ideaName, ideaDescription, userID, (int)ideaStatus, (int)workflowStep, assignedTo);
            newIdea.Load(GetRecords(insertStatememt));
            AutomationCore.base_tests.BaseTest.WriteReport("Idea " + newIdea.Rows[0][0] + " created...");
            return int.Parse(newIdea.Rows[0][0].ToString());
        }

        public int CreateSavedIdea(string ideaName, string ideaDescription, string userID = "", string contactName = "Automation", string contactPhone = "515-555-1212", string contactEmail = "hpg.automation@hcahealthcare.com")
        {
            return CreateNewIdea(ideaName, ideaDescription);
        }

        public void SubmitIdea(int ideaNumber)
        {
            //int rAff = ExecuteStatement(
            //    string.Format(
            //    "INSERT INTO HPG_IdeaManagementSBX.dbo.QualifiedIdeas ([IdeaId] ,[Title] ,[Description] ,[StatusId] ,[MembershipTypeId] ,[CategoryId] ,[DepartmentId] ,[ImpactLevel] ,[EffortLevel] ,[PublishedDate] ,[CreatedDate] ,[UpdatedDate]) VALUES ({0}), (SELECT [Title] FROM Ideas WHERE IdeaID = {0}), (SELECT [Description] FROM Ideas WHERE IdeaID = {0}), 2, 3, 1, 1, 1, 1, GETDATE(), GETDATE(), GETDATE());",
            //    ideaNumber));
            int rAff = ExecuteStatement("UPDATE Ideas SET StatusID = 2 WHERE IdeaId = " + ideaNumber.ToString() + ";");
            HpgAssert.AreEqual("1", rAff.ToString(), "Verify 1 record (idea) updated to Accepted");
        }

        public void AcceptIdea(int ideaNumber)
        {
            int rAff = ExecuteStatement("UPDATE Ideas SET StatusID = 7 WHERE IdeaId = " + ideaNumber.ToString() + ";");
            HpgAssert.AreEqual("1", rAff.ToString(), "Verify 1 record (idea) updated to Accepted");
        }

        public int PublishQualifiedIdea(int ideaNumber)
        {
            return PublishQualifiedIdea(ideaNumber, new int[]{2});
        }

        public int PublishQualifiedIdea(int ideaNumber, int[] MembershipTypeIds)
        {
            return ExecuteStatement("UPDATE QualifiedIdeas SET StatusId = 2, PublishedDate = GETDATE() WHERE MembershipTypeId IN (" + string.Join(",", MembershipTypeIds) + ") AND IdeaId = " + ideaNumber.ToString() + ";");
        }

        public void AssignIdeaTo(int ideaId, string UserId)
        {
            if (ExecuteStatement(string.Format("UPDATE Ideas SET AssignTo = '{0}' WHERE IdeaId = {1};",
                                               UserId.Trim().ToUpper(),
                                               ideaId.ToString())) < 1)
            {
                AutomationCore.SuperTest.WriteReport(string.Format("** WARNING! AssignIdeaTo {0} for idea {1} yeilded 0 records affected! **", UserId, ideaId.ToString()));
            }
        }

        //public DataTable GetStreetwiseIdeas()
        //{
            
        //}
    }
}

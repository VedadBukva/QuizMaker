#pragma warning disable 1591

namespace QuizMaker.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.QuizQuestions", new[] { "QuizId" });
            RenameIndex(table: "dbo.QuizQuestions", name: "IX_QuestionId", newName: "IX_QuizQuestion_QuestionId");
            AlterColumn("dbo.Questions", "Text", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Questions", "CorrectAnswer", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Quizzes", "Name", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Questions", "Text", name: "IX_Question_Text");
            CreateIndex("dbo.Questions", "CreatedAtUtc", name: "IX_Question_CreatedAtUtc");
            CreateIndex("dbo.QuizQuestions", new[] { "QuizId", "DisplayOrder" }, unique: true, name: "UX_QuizQuestion_QuizId_DisplayOrder");
            CreateIndex("dbo.Quizzes", "Name", name: "IX_Quiz_Name");
            CreateIndex("dbo.Quizzes", new[] { "IsDeleted", "CreatedAtUtc" }, name: "IX_Quiz_IsDeleted_CreatedAtUtc");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Quizzes", "IX_Quiz_IsDeleted_CreatedAtUtc");
            DropIndex("dbo.Quizzes", "IX_Quiz_Name");
            DropIndex("dbo.QuizQuestions", "UX_QuizQuestion_QuizId_DisplayOrder");
            DropIndex("dbo.Questions", "IX_Question_CreatedAtUtc");
            DropIndex("dbo.Questions", "IX_Question_Text");
            AlterColumn("dbo.Quizzes", "Name", c => c.String());
            AlterColumn("dbo.Questions", "CorrectAnswer", c => c.String());
            AlterColumn("dbo.Questions", "Text", c => c.String());
            RenameIndex(table: "dbo.QuizQuestions", name: "IX_QuizQuestion_QuestionId", newName: "IX_QuestionId");
            CreateIndex("dbo.QuizQuestions", "QuizId");
        }
    }
}

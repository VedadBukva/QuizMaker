#pragma warning disable 1591

namespace QuizMaker.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlignQuizTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Quizs", newName: "Quizzes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Quizzes", newName: "Quizs");
        }
    }
}

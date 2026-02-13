using QuizMaker.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace QuizMaker.Infrastructure.Data
{
    /// <inheritdoc />
    public class QuizMakerDbContext : DbContext
    {
        /// <inheritdoc />
        public QuizMakerDbContext() : base("name=QuizMakerDb")
        {
        }

        /// <summary>
        /// Quiz entity.
        /// </summary>
        public DbSet<Quiz> Quizzes { get; set; }

        /// <summary>
        /// Question entity.
        /// </summary>
        public DbSet<Question> Questions { get; set; }

        /// <summary>
        /// QuizQuestions entity.
        /// </summary>
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>().ToTable("Quizzes");

            modelBuilder.Entity<QuizQuestion>()
                .HasKey(q => new { q.QuizId, q.QuestionId });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quiz>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Quiz>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Quiz_Name"))
                );

            modelBuilder.Entity<Quiz>()
                .Property(x => x.IsDeleted)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Quiz_IsDeleted_CreatedAtUtc", 1))
                );

            modelBuilder.Entity<Quiz>()
                .Property(x => x.CreatedAtUtc)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Quiz_IsDeleted_CreatedAtUtc", 2))
                );

            modelBuilder.Entity<Question>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Question>()
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Question_Text"))
                );

            modelBuilder.Entity<Question>()
                .Property(x => x.CorrectAnswer)
                .IsRequired()
                .HasMaxLength(1000);

            modelBuilder.Entity<Question>()
                .Property(x => x.CreatedAtUtc)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Question_CreatedAtUtc"))
                );

            modelBuilder.Entity<QuizQuestion>()
                .HasKey(x => new { x.QuizId, x.QuestionId });

            modelBuilder.Entity<QuizQuestion>()
                .HasRequired(x => x.Quiz)
                .WithMany(x => x.QuizQuestions)
                .HasForeignKey(x => x.QuizId);

            modelBuilder.Entity<QuizQuestion>()
                .HasRequired(x => x.Question)
                .WithMany(x => x.QuizQuestions)
                .HasForeignKey(x => x.QuestionId);

            modelBuilder.Entity<QuizQuestion>()
                .Property(x => x.QuizId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_QuizQuestion_QuizId"))
                );

            modelBuilder.Entity<QuizQuestion>()
                .Property(x => x.QuestionId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_QuizQuestion_QuestionId"))
                );

            modelBuilder.Entity<QuizQuestion>()
                .Property(x => x.QuizId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UX_QuizQuestion_QuizId_DisplayOrder", 1) { IsUnique = true }
                    )
                );

            modelBuilder.Entity<QuizQuestion>()
                .Property(x => x.DisplayOrder)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UX_QuizQuestion_QuizId_DisplayOrder", 2) { IsUnique = true }
                    )
                );
        }
    }
}

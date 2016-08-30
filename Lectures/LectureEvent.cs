using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCCDatabase.Lectures
{

	public interface ILectureEvent
	{
		Guid? Id { get; set; }
		DateTime? Start { get; set; }
		DateTime? End { get; set; }
		string Presenter { get; set; }
		string Description { get; set; }
		string Location { get; set; }
		int? StatusId { get; set; }
	}

	/// <summary>Inputs to create a new lecture event
	/// </summary>
	public class LectureEventEditor : IValidatableObject, ILectureEvent
	{
		public Guid? Id { get; set; }

		[Required(ErrorMessage = "Enter the date and time of the lecture's start.")]
		[Display(Name = "Start Time", Description = "Date and time the lecture starts, must be before the end time.")]
		public DateTime? Start { get; set; }

		[Required(ErrorMessage = "Enter the date and time of the lecture's end.")]
		[Display(Name = "End Time", Description = "Date and time the lecture ends, must be after the start time.")]
		public DateTime? End { get; set; }

		[Required(ErrorMessage = "Please enter the names of the presenters.")]
		[Display(Description = "Name(s) of the lecture presenter.")]
		public string Presenter { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Description = "Describe the nature of the lecture.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "A location must be provided.")]
		[Display(Description = "Where will the lecture be held?")]
		public string Location { get; set; }

		public int? StatusId { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResults = new List<ValidationResult>();

			if(Start > End)
			{
				validationResults.Add(
					new ValidationResult(
						"The lecture must start before it can end. Double check the times.",
						new List<string> { "Start", "End" }
					)
				);
			}

			return validationResults;
		}

		public static implicit operator LectureEventEditor(LectureEvent l)
		{
			if(l == null)
			{
				return null;
			}

			return new LectureEventEditor
			{
				Id = l.Id,
				Start = l.Start,
				End = l.End,
				Location = l.Location,
				Description = l.Description,
				Presenter = l.Presenter,
				StatusId = l.StatusId
			};
		}
	}

	/// <summary>Individual transaction that modified a lecture event
	/// </summary>
	public class LectureEventHistory : ILectureEvent
	{
		public Guid? Id { get; set; }
		public DateTime? Start { get; set; }
		public DateTime? End { get; set; }
		public string Presenter { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Status { get; set; }
		public int? ModifiedById { get; set; }
		public string ModifiedBy { get; set; }
		public int? StatusId { get; set; }
	}

	/// <summary>The current iteration of a lecture event
	/// </summary>
	public class LectureEvent : ILectureEvent
	{
		public Guid? Id { get; set; }

		public DateTime? Created { get; set; }
		public DateTime? Start { get; set; }
		public DateTime? End { get; set; }
		public DateTime? Modified { get; set; }

		public string Presenter { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedBy { get; set; }
		public string Status { get; set; }

		public int? StatusId { get; set; }
		public int? CreatedById { get; set; }
		public int? ModifiedById { get; set; }

		public List<LectureEventHistory> History { get; set; }

		public LectureEvent()
		{

		}

		public LectureEvent(DbDataReader reader)
		{
			try
			{
				Id = reader["EventId"] as Guid?;

				Presenter = reader["Presenter"] as string;
				Description = reader["Description"] as string;
				Location = reader["Location"] as string;
				ModifiedBy = reader["ModifiedBy"] as string;
				CreatedBy = reader["CreatedBy"] as string;
				Status = reader["Status"] as string;

				Created = reader["CreatedDate"] as DateTime?;
				Start = reader["Start"] as DateTime?;
				End = reader["End"] as DateTime?;
				Modified = reader["ModifiedDate"] as DateTime?;

				StatusId = reader["StatusId"] as int?;
				CreatedById = reader["CreatedById"] as int?;
				ModifiedById = reader["ModifiedById"] as int?;
			}
			finally
			{

			}
		}
	}
}

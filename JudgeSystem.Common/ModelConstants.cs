﻿namespace JudgeSystem.Common
{
    public static class ModelConstants
    {
        #region User models constants
        public const int UserFirstNameMaxLength = 30;
        public const int UserSurnameMaxLength = 30;
        public const int UserPasswordMinLength = 5;
        public const int UserPasswordMaxLength = 50;
        public const string UserNewPasswordDisplayName = "New password";
        public const string UserOldPasswordDisplayName = "Current password";
        public const string UserConfirmPasswordDisplayName = "Confirm new password";
        public const string UserPhoneNumberDisplayName = "Phone number";
        #endregion

        #region Contest models consatnts
        public const int ContestNameMaxLength = 50;
        public const int ContestNameMinLength = 5;
        public const string ContestStartTimeDisplayName = "Start time";
        public const string ContestEndTimeDisplayName = "End time";
        public const string ContestLessonIdDisplayName = "Lesson";
        #endregion

        #region Lesson models consatnts
        public const int LessonNameMaxLength = 50;
        public const int LessonNameMinLength = 5;
        public const int LessonPasswordMinLength = 5;
        public const int LessonPasswordMaxLength = 50;
        public const string LessonPasswordDisplayName = "Lesson password";
        public const string LessonOldPasswordDisplayName = "Old lesson password";
        #endregion

        #region Course models consatnts
        public const int CourseNameMaxLength = 50;
        public const int CourseNameMinLength = 3;
        #endregion

        #region Problem models consatnts
        public const int ProblemNameMaxLength = 30;
        public const int ProblemNameMinLength = 3;
        public const int ProblemMinPoints = 1;
        public const int ProblemMaxPoints = 300;
        public const string ProblemIsExtraTaskDisplayName = "Extra task";
        public const string ProblemMaxPointsDisplayName = "Max points";
        public const string ProblemAllowedTimeInMillisecondsDisplayName = "Allowed time in miliseconds";
        public const string ProblemAllowedMemoryInMegaBytesDisplayName = "Allowed memory in MB";
        public const string ProblemSubmissionTypeDisplayName = "Submission type";
        #endregion

        #region Resource models consatnts
        public const int ResourceNameMaxLength = 30;
        public const int ResourceNameMinLength = 3;
        #endregion

        #region Student models consatnts
        public const int StudentFullNameMaxLength = 50;
        public const int StudentFullNameMinLength = 10;
        public const int StudentEmailMaxLength = 30;
        public const string StudentActivationKeyDisplayName = "Activation key";
        public const string StudentFullNameDisplayName = "Full name";
        public const string StudentSchoolClassIdDisplayName = "Class";
        public const string StudentNumberInClassDisplayName = "Number in class";

        #endregion

        #region Test models consatnts
        public const int TestInputDataMaxLength = 10000;
        public const int TestOutputDataMaxLength = 10000;
        public const string TestInputDataDisplayName = "Input";
        public const string TestOutputDataDisplayName = "Expected output";
        public const string TestIsTrialTestDisplayName = "Trial test";
        #endregion
    }
}

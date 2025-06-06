 
/****** Object:  Database [EduSyncProjectDB]    Script Date: 5/23/2025 1:57:52 PM ******/
CREATE DATABASE [EduSyncProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EduSyncProjectDB', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EduSyncProjectDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EduSyncProjectDB_log', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EduSyncProjectDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
 
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assessment]    Script Date: 5/23/2025 1:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assessment](
	[AssessmentId] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NULL,
	[Title] [varchar](200) NOT NULL,
	[Questions] [nvarchar](max) NULL,
	[MaxScore] [int] NULL,
 CONSTRAINT [PK__Assessme__3D2BF81E2E1CFF82] PRIMARY KEY CLUSTERED 
(
	[AssessmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssessmentResult]    Script Date: 5/23/2025 1:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssessmentResult](
	[ResultId] [uniqueidentifier] NOT NULL,
	[AssessmentId] [uniqueidentifier] NOT NULL,
	[StudentId] [uniqueidentifier] NOT NULL,
	[Score] [int] NOT NULL,
	[MaxScore] [int] NOT NULL,
	[SubmissionDate] [datetime] NOT NULL,
	[Answers] [varchar](max) NULL,
 CONSTRAINT [PK_AssessmentResults] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 5/23/2025 1:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [uniqueidentifier] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Description] [varchar](100) NULL,
	[InstructorId] [uniqueidentifier] NULL,
	[MediaUrl] [varchar](500) NULL,
 CONSTRAINT [PK__Course__C92D71A72D95F113] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Result]    Script Date: 5/23/2025 1:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Result](
	[ResultId] [uniqueidentifier] NOT NULL,
	[AssessmentId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[Score] [int] NULL,
	[AttemptDate] [datetime] NULL,
 CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserModel]    Script Date: 5/23/2025 1:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserModel](
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Role] [varchar](20) NULL,
	[PasswordHash] [varchar](225) NOT NULL,
 CONSTRAINT [PK__UserMode__1788CC4C26566554] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250523064425_AddAssessmentResults', N'9.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250523080558_InitialCreate', N'9.0.5')
GO
/****** Object:  Index [IX_Assessment_CourseId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_Assessment_CourseId] ON [dbo].[Assessment]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssessmentResult_AssessmentId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_AssessmentResult_AssessmentId] ON [dbo].[AssessmentResult]
(
	[AssessmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssessmentResult_StudentId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_AssessmentResult_StudentId] ON [dbo].[AssessmentResult]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Course_InstructorId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_Course_InstructorId] ON [dbo].[Course]
(
	[InstructorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Result_AssessmentId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_Result_AssessmentId] ON [dbo].[Result]
(
	[AssessmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Result_UserId]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_Result_UserId] ON [dbo].[Result]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_UserModels_Email]    Script Date: 5/23/2025 1:57:53 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_UserModels_Email] ON [dbo].[UserModel]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Assessment] ADD  DEFAULT (newsequentialid()) FOR [AssessmentId]
GO
ALTER TABLE [dbo].[AssessmentResult] ADD  DEFAULT (newsequentialid()) FOR [ResultId]
GO
ALTER TABLE [dbo].[Course] ADD  DEFAULT (newsequentialid()) FOR [CourseId]
GO
ALTER TABLE [dbo].[Result] ADD  DEFAULT (newsequentialid()) FOR [ResultId]
GO
ALTER TABLE [dbo].[UserModel] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[Assessment]  WITH CHECK ADD  CONSTRAINT [FK__Assessmen__Cours__3E52440B] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[Assessment] CHECK CONSTRAINT [FK__Assessmen__Cours__3E52440B]
GO
ALTER TABLE [dbo].[AssessmentResult]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentResult_Assessment] FOREIGN KEY([AssessmentId])
REFERENCES [dbo].[Assessment] ([AssessmentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssessmentResult] CHECK CONSTRAINT [FK_AssessmentResult_Assessment]
GO
ALTER TABLE [dbo].[AssessmentResult]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentResult_UserModel] FOREIGN KEY([StudentId])
REFERENCES [dbo].[UserModel] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssessmentResult] CHECK CONSTRAINT [FK_AssessmentResult_UserModel]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_UserModel] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[UserModel] ([UserId])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_UserModel]
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD  CONSTRAINT [FK_Result_Assessment] FOREIGN KEY([AssessmentId])
REFERENCES [dbo].[Assessment] ([AssessmentId])
GO
ALTER TABLE [dbo].[Result] CHECK CONSTRAINT [FK_Result_Assessment]
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD  CONSTRAINT [FK_Result_UserModel] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserModel] ([UserId])
GO
ALTER TABLE [dbo].[Result] CHECK CONSTRAINT [FK_Result_UserModel]
GO


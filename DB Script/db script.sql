USE [master]
GO
CREATE DATABASE [TestChat]
GO
USE [TestChat]
GO
CREATE TABLE [dbo].[UserChats](
	[Id] [uniqueidentifier] NOT NULL,
	[SenderId] [uniqueidentifier] NOT NULL,
	[RecieverId] [uniqueidentifier] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[MessageTime] [datetime] NOT NULL,
	[ReadStatus] [bit] NOT NULL,
 CONSTRAINT [PK_UserChats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserChats] ADD  CONSTRAINT [DF_UserChats_ReadStatus]  DEFAULT ((1)) FOR [ReadStatus]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserChats]  WITH CHECK ADD  CONSTRAINT [FK_UserChats_Receiver] FOREIGN KEY([RecieverId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserChats] CHECK CONSTRAINT [FK_UserChats_Receiver]
GO
ALTER TABLE [dbo].[UserChats]  WITH CHECK ADD  CONSTRAINT [FK_UserChats_Sender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserChats] CHECK CONSTRAINT [FK_UserChats_Sender]

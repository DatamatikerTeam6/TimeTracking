USE [DogRallyContext3]
GO

/****** Object: View [dbo].[TrackExerciseView] Script Date: 08-05-2024 10:51:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[TrackExerciseView]
    AS SELECT
    te.ForeignTrackID, te.ForeignExerciseID, te.TrackExercisePositionX, te.TrackExercisePositionY,
    t.TrackName, e.ExerciseName, e.ExerciseIllustrationPath
    FROM
    TrackExercise te
    INNER JOIN
    Track t ON te.ForeignTrackID = t.TrackID
    INNER JOIN
    Exercise e ON te.ForeignExerciseID = e.ExerciseID

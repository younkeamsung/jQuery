using JqueryMovieApi.Dtos;
using JqueryMovieApi.Services;
using System.Runtime.CompilerServices;

namespace JqueryMovieApi.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovieEndpoints(this WebApplication app)
        {
            app.MapGet("/api/movies", async (MovieService movieService) =>
            {
                var movies = await movieService.GetMovies();

                return Results.Ok(movies);
            });

            app.MapGet("/api/movies/{id:int}", async (int id, MovieService movieService) =>
            {
                var movie = await movieService.GetMovie(id);

                if(movie == null)
                {
                    return Results.NotFound(new
                    {
                        message = "해당 영화를 찾을 수 없습니다."
                    });
                }

                return Results.Ok(movie);
            });

            app.MapGet("/api/movies/search", async (string title, MovieService movieService) =>
            {
                var movies = await movieService.SearchMovies(title);

                return Results.Ok(movies);
            });

            app.MapPost("/api/movies", async (MovieDetailsDto dto, MovieService movieService) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return Results.BadRequest(new
                    {
                        message = "영화 제목은 필수입니다."
                    });
                }

                if(dto.Year < 1900 || dto.Year > 2100)
                {
                    return Results.BadRequest(new
                    {
                        message = "개봉년도는 1900년부터 2100년까지 입력해주세요."
                    });
                }
                var result = await movieService.CreateMovie(dto);
                return Results.Created($"/api/movies/{result.Id}", result);
            });

            app.MapDelete("/api/movies/{id:int}", async (int id, MovieService movieService) =>
            {
                var result = await movieService.DeleteMovie(id);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "해당 영화를 찾을 수 없습니다."
                    });
                }
                return Results.Ok(new
                {
                    message = "영화가 삭제되었습니다."
                });
            });

            app.MapPut("/api/movies/{id:int}", async (int id, MovieDetailsDto dto, MovieService movieService) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return Results.BadRequest(new
                    {
                        message = "영화 제목은 필수입니다."
                    });
                }

                if (dto.Year < 1900 || dto.Year > 2100)
                {
                    return Results.BadRequest(new
                    {
                        message = "개봉년도는 1900년부터 2100년까지 입력해주세요."
                    });
                }

                var result = await movieService.UpdateMovie(id, dto);

                if(result == null)
                {
                    return Results.NotFound(new
                    {
                        message = "해당 영화를 찾을 수 없습니다."
                    });
                }
                return Results.Ok(result);
            });
        }
    }
}

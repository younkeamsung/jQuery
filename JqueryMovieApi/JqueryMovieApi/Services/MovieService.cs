using JqueryMovieApi.Data;
using JqueryMovieApi.Dtos;
using JqueryMovieApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JqueryMovieApi.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<MovieDto>> GetMovies()
        {
            return await _context.Movies
                .Select(movie => new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year
                })
                .ToListAsync();
        }
        public async Task<MovieDto?> GetMovie(int id)
        {
            return await _context.Movies
                .Where(movie => movie.Id == id)
                .Select(movie => new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MovieDto> CreateMovie(MovieDetailsDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year
            };

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year
            };
        }

        public async Task<MovieDto?> UpdateMovie(int id, MovieDetailsDto dto)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if(movie == null)
            {
                return null;
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;

            await _context.SaveChangesAsync();

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year
            };
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if(movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<MovieDto>> SearchMovies(string title)
        {
            return await _context.Movies
                .Where(movie => movie.Title.Contains(title))
                .Select(movie => new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year
                })
                .ToListAsync();
        }
    }
}

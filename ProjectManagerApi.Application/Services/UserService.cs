using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces; // Para IUserRepository
using ProjectManagerApi.Application.Services.Interfaces; // Para IUserService
using ProjectManagerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto userDto)
        {
            // TODO: Implementar lógica de HASH DE SENHA antes de criar a entidade.
            // Isso é CRÍTICO para a segurança.
            // Ex: string hashedPassword = YourPasswordHasher.HashPassword(userDto.Password);

            // IMPORTANTE: Sua entidade User tem um construtor que espera um passwordHash, não a senha em texto claro.
            // O DTO 'CreateUserDto' provavelmente tem uma propriedade 'Password'.
            // Você precisará mapear isso manualmente aqui ou garantir que o AutoMapper saiba como lidar com isso.
            // Por agora, assumindo que CreateUserDto.Password será o que você usaria para gerar o hash.

            // Exemplo ajustado para usar o construtor da entidade User com a senha já 'hasheada'
            // Supondo que userDto.Password é o texto claro da senha que precisa ser hasheado
            // String dummyHashedPassword = userDto.Password; // Substitua por BCrypt.Net-Next ou similar

            var user = new User(userDto.Name, userDto.Email, userDto.Password); // Temporariamente usando userDto.Password diretamente. MUDAR ISSO!
            // CORREÇÃO FUTURA: var user = new User(userDto.Name, userDto.Email, hashedPassword);

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            // Requisito: IUserRepository.cs PRECISA ter um método Task<IEnumerable<User>> GetAllAsync();
            // Sem ele, esta linha resultará em erro de compilação ou de tempo de execução.
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(Guid id, UpdateUserDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return null; // Usuário não encontrado
            }

            // Atualizando as propriedades da entidade User usando seus métodos internos.
            // Isso garante a integridade e as regras de negócio da entidade.

            // Apenas atualiza o nome se um novo nome for fornecido no DTO
            if (userDto.Name != null)
            {
                existingUser.UpdateName(userDto.Name);
            }

            // !!! ATENÇÃO: Sua entidade User.cs NÃO possui um método UpdateEmail(string newEmail).
            // Se você deseja permitir a atualização do email, precisa adicionar o método
            // public void UpdateEmail(string newEmail) { /* validação e atribuição */ }
            // na sua entidade User.cs.
            // Por enquanto, esta parte está comentada para evitar erro de compilação.
            /*
            if (userDto.Email != null)
            {
                // TODO: Adicionar lógica para verificar se o novo email já está em uso por outro usuário.
                // Exemplo: if (await _userRepository.GetByEmailAsync(userDto.Email) != null && userDto.Email != existingUser.Email) { throw new Exception("Email already taken."); }
                existingUser.UpdateEmail(userDto.Email);
            }
            */

            // Apenas atualiza a senha se uma nova senha for fornecida no DTO
            if (userDto.Password != null)
            {
                // TODO: Gerar o HASH DA NOVA SENHA antes de atualizar.
                // Ex: string newHashedPassword = YourPasswordHasher.HashPassword(userDto.Password);
                existingUser.UpdatePasswordHash(userDto.Password); // Temporariamente usando userDto.Password diretamente. MUDAR ISSO!
                // CORREÇÃO FUTURA: existingUser.UpdatePasswordHash(newHashedPassword);
            }

            await _userRepository.UpdateAsync(existingUser);
            return _mapper.Map<UserResponseDto>(existingUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false; // Usuário não encontrado
            }
            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}
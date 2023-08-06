using AutoMapper;
using Digital.Base.Model;
using Digital.Base.Response;
using Digital.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Base
{
    public class BaseService<TEntity, TRequest, TResponse> : IBaseService<TEntity, TRequest, TResponse> where TEntity : BaseModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public virtual ApiResponse Delete(int Id)
        {
            try
            {
                var entity = unitOfWork.GetRepository<TEntity>().GetById(Id);
                if (entity is null)
                {
                    return new ApiResponse("Record not found");
                }

                unitOfWork.GetRepository<TEntity>().DeleteById(Id);
                if (unitOfWork.SaveChanges() > 0)
                {
                    return new ApiResponse();
                }
                return new ApiResponse("Internal Server Error");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual ApiResponse<List<TResponse>> GetAll()
        {
            try
            {
                var entityList = unitOfWork.GetRepository<TEntity>().GetAll()?.ToList();
                var mapped = mapper.Map<List<TEntity>, List<TResponse>>(entityList);
                return new ApiResponse<List<TResponse>>(mapped);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TResponse>>(ex.Message);
            }
        }

        public virtual ApiResponse<TResponse> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetRepository<TEntity>().GetById(id);
                if (entity is null)
                {
                    return new ApiResponse<TResponse>("Record not found");
                }

                var mapped = mapper.Map<TEntity, TResponse>(entity);
                return new ApiResponse<TResponse>(mapped);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResponse>(ex.Message);
            }
        }

        public virtual ApiResponse Insert(TRequest request)
        {
            try
            {
                var entity = mapper.Map<TRequest, TEntity>(request);
                entity.CreatedAt = DateTime.UtcNow;
                unitOfWork.GetRepository<TEntity>().Add(entity);
                if (unitOfWork.SaveChanges() > 0)
                {
                    return new ApiResponse();
                }
                return new ApiResponse("Internal Server Error");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual ApiResponse Update(int Id, TRequest request)
        {
            try
            {
                var entity = mapper.Map<TRequest, TEntity>(request);

                var exist = unitOfWork.GetRepository<TEntity>().GetAllAsNoTracking(x => x.Id.Equals(Id))?.FirstOrDefault();
                if (exist is null)
                {
                    return new ApiResponse("Record not found");
                }

                entity.Id = Id;
                entity.UpdatedAt = DateTime.UtcNow;

                unitOfWork.GetRepository<TEntity>().Update(entity);
                if (unitOfWork.SaveChanges() > 0)
                {
                    return new ApiResponse();
                }
                return new ApiResponse("Internal Server Error");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }

}

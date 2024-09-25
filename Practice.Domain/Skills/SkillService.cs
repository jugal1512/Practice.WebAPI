using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Skills;
public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;
    public SkillService(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<List<Skill>> GetSkillsByEmployeeId(int Id)
    {
        return await _skillRepository.GetSkillsByEmployeeId(Id);
    }
    public async Task<List<Skill>> DeleteSkills(int Id)
    {
        return await _skillRepository.DeleteSkills(Id);
    }
}

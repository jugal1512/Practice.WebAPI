using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Skills;
public interface ISkillService
{
    public Task<List<Skill>> GetSkillsByEmployeeId(int Id);
    public Task<List<Skill>> DeleteSkills(int Id);

}

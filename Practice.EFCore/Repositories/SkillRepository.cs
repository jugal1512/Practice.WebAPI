using Microsoft.EntityFrameworkCore;
using Practice.Domain.Skills;
using Practice.EFCore.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Repositories;
public class SkillRepository : ISkillRepository
{
    private readonly ApplicationDbContext _context;
    public SkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Skill>> GetSkillsByEmployeeId(int Id)
    {
        var skills = await _context.Skills.Where(s => s.EmployeeId == Id).ToListAsync();
        return skills;
    }

    public async Task<List<Skill>> DeleteSkills(int Id)
    {
        var getSkillsByEmployeeId = await GetSkillsByEmployeeId(Id);
        if (getSkillsByEmployeeId != null && getSkillsByEmployeeId.Count() > 0)
        {
            _context.Skills.RemoveRange(getSkillsByEmployeeId);
            await _context.SaveChangesAsync();
        }
        return getSkillsByEmployeeId;
    }
}

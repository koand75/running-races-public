import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DragDropModule, CdkDragDrop } from '@angular/cdk/drag-drop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SectionService } from '../../services/section.service';
import { RunnerService } from '../../services/runner.service';
import { RunnerSectionService } from '../../services/runner-section.service';
import { Section, Runner, RunnerSection } from '../../models/ub.models';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/ub.models';

interface RunnerStats {
    runner: Runner;
    blocks: { km: number; sections: number; startOrder: number }[];
    totalKm: number;
}

interface SectionBlock {
    runnerId: number | null;
    sections: Section[];
    totalKm: number;
}

@Component({
    selector: 'app-planner',
    standalone: true,
    imports: [
        CommonModule,
        RouterLink,
        FormsModule,
        DragDropModule,
        MatButtonModule,
        MatIconModule,
        MatInputModule,
        MatFormFieldModule
    ],
    templateUrl: './planner.html',
    styleUrl: './planner.css'
})
export class PlannerComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private sectionService = inject(SectionService);
    private runnerService = inject(RunnerService);
    private runnerSectionService = inject(RunnerSectionService);
    private teamService = inject(TeamService);

    teamId!: number;
    sections: Section[] = [];
    runners: Runner[] = [];
    assignments: Map<number, RunnerSection> = new Map(); 
    runnerStats: Map<number, RunnerStats> = new Map(); 

    selectedRunner: Runner | null = null;
    team: Team | null = null;
    hasChanges = false;
    readonly DEFAULT_PACE = 390; // 6:30/km másodpercben
    selectedBlock: SectionBlock | null = null;

    ngOnInit(): void {
        this.teamId = Number(this.route.snapshot.paramMap.get('id'));
        this.teamService.getById(this.teamId).subscribe(t => this.team = t);
        this.loadData();
    }

    loadData(): void {
        this.sectionService.getAll().subscribe(sections => {
            this.sections = sections;
        });

        this.runnerService.getByTeam(this.teamId).subscribe(runners => {
            this.runners = runners;
            this.calculateAllStats();
        });

        this.runnerSectionService.getByTeam(this.teamId).subscribe(assignments => {
            this.assignments.clear();
            assignments.forEach(a => this.assignments.set(a.sectionId, a));
            this.calculateAllStats();
        });
    }

    selectRunner(runner: Runner): void {
        this.selectedRunner = this.selectedRunner?.id === runner.id ? null : runner;
    }
   
    onSectionClick(section: Section): void {

        if (!this.selectedRunner) return;

        const existing = this.assignments.get(section.id);
        if (existing?.runnerId === this.selectedRunner.id) {            
            this.assignments.delete(section.id);
        } else {            
            this.assignments.set(section.id, {
                sectionId: section.id,
                runnerId: this.selectedRunner.id,
                customPace: this.selectedRunner.basePace,
                runner: this.selectedRunner,
                section: section
            });
        }
        this.hasChanges = true;
        this.calculateAllStats();
    }

    // Drag & drop
    onDrop(event: CdkDragDrop<any>, section: Section): void {
       
        const runner = event.item.data as Runner;     

        this.assignments.set(section.id, {
            sectionId: section.id,
            runnerId: runner.id,
            customPace: runner.basePace,
            runner: runner,
            section: section
        });

        this.hasChanges = true;
        this.calculateAllStats();
    }

    removeAssignment(sectionId: number): void {
        this.assignments.delete(sectionId);
        this.hasChanges = true;
        this.calculateAllStats();
    }

    updateCustomPace(sectionId: number, pace: number): void {
        const assignment = this.assignments.get(sectionId);
        if (assignment) {
            assignment.customPace = pace;
            this.hasChanges = true;
        }
    }

    calculateAllStats(): void {
        this.runnerStats.clear();

        this.runners.forEach(runner => {
            this.runnerStats.set(runner.id, {
                runner,
                blocks: [],
                totalKm: 0
            });
        });

        let currentRunnerId: number | null = null;
        let currentBlock = { km: 0, sections: 0, startOrder: 0 };

        this.sections.forEach(section => {
            const assignment = this.assignments.get(section.id);

            if (assignment) {
                const stats = this.runnerStats.get(assignment.runnerId)!;
                if (!stats) return;
                
                if (currentRunnerId === assignment.runnerId) {                   
                    currentBlock.km += section.distance;
                    currentBlock.sections++;
                } else {                  
                    if (currentRunnerId !== null && currentBlock.sections > 0) {
                        const prevStats = this.runnerStats.get(currentRunnerId)!;
                        prevStats.blocks.push({ ...currentBlock });
                    }                   
                    currentRunnerId = assignment.runnerId;
                    currentBlock = { km: section.distance, sections: 1, startOrder: section.order };
                }

                stats.totalKm += section.distance;
            } else {             
                if (currentRunnerId !== null && currentBlock.sections > 0) {
                    const prevStats = this.runnerStats.get(currentRunnerId)!;
                    prevStats.blocks.push({ ...currentBlock });
                }
                currentRunnerId = null;
                currentBlock = { km: 0, sections: 0, startOrder: 0 };
            }
        });

        if (currentRunnerId !== null && currentBlock.sections > 0) {
            const prevStats = this.runnerStats.get(currentRunnerId)!;
            prevStats.blocks.push({ ...currentBlock });
        }
    }

    formatStats(runnerId: number): string {
        const stats = this.runnerStats.get(runnerId);
        if (!stats || stats.blocks.length === 0) return '–';

        const blocksStr = stats.blocks
            .map(b => `${this.getCumulativeTimeAbsolute(b.startOrder)} (${b.km.toFixed(1)}km/${b.sections}sz)`)
            .join(' + ');

        return `${blocksStr} = ${stats.totalKm.toFixed(1)} km`;
    }

    getRunnerBlocks(runnerId: number): string[] {
        const stats = this.runnerStats.get(runnerId);
        if (!stats || stats.blocks.length === 0) return ['–'];
        const blocks = stats.blocks.map(b =>
            `${this.getCumulativeTimeAbsolute(b.startOrder)} (${b.km.toFixed(1)}km/${b.sections}sz)`
        );
        blocks.push(`Összesen: ${stats.totalKm.toFixed(1)} km`);
        return blocks;
    }

    formatPace(seconds: number): string {
        const min = Math.floor(seconds / 60);
        const sec = seconds % 60;
        return `${min}:${sec.toString().padStart(2, '0')}`;
    }

    getRunnerColor(runnerId: number): string {
        const colors = [
            '#078080', '#f45d48', '#6c5ce7', '#00b894',
            '#fdcb6e', '#e17055', '#0984e3', '#b2bec3',
            '#d63031', '#00cec9', '#e84393', '#2d3436', '#fab1a0'
        ];
        const index = this.runners.findIndex(r => r.id === runnerId);
        return colors[index % colors.length];
    }

    getRunnerName(runnerId: number): string {
        const runner = this.runners.find(r => r.id === runnerId);
        return runner?.name || '?';
    }

    parsePace(pace: string): number {
        const parts = pace.split(':');
        const min = parseInt(parts[0]) || 0;
        const sec = parseInt(parts[1]) || 0;
        return min * 60 + sec;
    }

    save(): void {
        const assignmentList = Array.from(this.assignments.values()).map(a => ({
            sectionId: a.sectionId,
            runnerId: a.runnerId,
            customPace: a.customPace
        }));

        this.runnerSectionService.saveAll(this.teamId, assignmentList as RunnerSection[])
            .subscribe(() => {
                this.hasChanges = false;
                alert('Mentve!');
            });
    }

    getCumulativeKm(sectionOrder: number): number {
        return this.sections
            .filter(s => s.order <= sectionOrder)
            .reduce((sum, s) => sum + Number(s.distance), 0);
    }

    getCumulativeTime(sectionOrder: number): string {
        const totalSeconds = this.sections
            .filter(s => s.order <= sectionOrder)
            .reduce((sum, s) => {
                const assignment = this.assignments.get(s.id);
                const pace = assignment?.customPace || this.DEFAULT_PACE;
                return sum + (Number(s.distance) * pace);
            }, 0);

        const h = Math.floor(totalSeconds / 3600);
        const m = Math.floor((totalSeconds % 3600) / 60);
        return `${h}:${m.toString().padStart(2, '0')}`;
    }

    getCumulativeTimeAbsolute(sectionOrder: number): string {
        if (!this.team?.startTime) return this.getCumulativeTime(sectionOrder);

        const totalSeconds = this.sections
            .filter(s => s.order < sectionOrder)  
            .reduce((sum, s) => {
                const assignment = this.assignments.get(s.id);
                const pace = assignment?.customPace || this.DEFAULT_PACE;
                return sum + (Number(s.distance) * pace);
            }, 0);

        const start = new Date(this.team.startTime);
        start.setSeconds(start.getSeconds() + totalSeconds);

        const days = ['V', 'H', 'K', 'Sze', 'Cs', 'P', 'Szo'];
        const day = days[start.getDay()];
        const h = start.getHours().toString().padStart(2, '0');
        const m = start.getMinutes().toString().padStart(2, '0');
        return `${day} ${h}:${m}`;
    }

    downloadTxt(): void {
        const lines: string[] = [];
        lines.push(`${this.team?.name} – ${this.team?.startTime}`);
        lines.push('');

        this.runners.forEach(runner => {
            const blocks = this.getRunnerBlocks(runner.id);
            if (blocks[0] !== '–') {
                lines.push(`${runner.name}: ${blocks.join(', ')}`);
            }
        });

        const blob = new Blob([lines.join('\n')], { type: 'text/plain' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.team?.name ?? 'beosztás'}.txt`;
        a.click();
        URL.revokeObjectURL(url);
    }

    getBlocks(): SectionBlock[] {
        const blocks: SectionBlock[] = [];
        let currentBlock: SectionBlock | null = null;


        this.sections.forEach(section => {
            const assignment = this.assignments.get(section.id);
            const runnerId = assignment?.runnerId ?? null;

            if (currentBlock && currentBlock.runnerId === runnerId) {
                currentBlock.sections.push(section);
                currentBlock.totalKm += Number(section.distance);
            } else {
                currentBlock = {
                    runnerId,
                    sections: [section],
                    totalKm: section.distance
                };
                blocks.push(currentBlock);
            }
        });

        return blocks;
    }

    onBlockClick(block: SectionBlock): void {
        if (!this.selectedBlock) {
            this.selectedBlock = block;
            return;
        }

        if (this.selectedBlock === block) {
            this.selectedBlock = null;
            return;
        }

        const idA = this.selectedBlock.runnerId;
        const idB = block.runnerId;

        this.selectedBlock.sections.forEach(s => {
            const a = this.assignments.get(s.id);
            if (idB === null) {
                this.assignments.delete(s.id);
            } else if (a) {
                a.runnerId = idB;
            }
        });

        block.sections.forEach(s => {
            const a = this.assignments.get(s.id);
            if (idA === null) {
                this.assignments.delete(s.id);
            } else if (a) {
                a.runnerId = idA;
            }
        });

        this.selectedBlock = null;
        this.hasChanges = true;
        this.calculateAllStats();
    }
}
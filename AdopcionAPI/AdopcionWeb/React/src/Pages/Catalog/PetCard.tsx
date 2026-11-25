import { Button, Card, CardFooter, Chip } from "@heroui/react";
import { PawIcon } from "@/components/PawIcon";
import {
  formatAge,
  formatVaccinationSummary,
  getPetImageUrl,
} from "@/lib/petHelpers";
import type { AnimalDto, VacunacionDto } from "@/types/models";

interface PetCardProps {
  animal: AnimalDto;
  especieNombre?: string;
  vacunaciones: VacunacionDto[];
  onSelectPet: (animal: AnimalDto) => void;
}

export function PetCard({
  animal,
  especieNombre,
  vacunaciones,
  onSelectPet,
}: PetCardProps) {
  const ageText = formatAge(animal.fechaNacimiento);
  const vaccineSummary = formatVaccinationSummary(
    animal.idAnimal,
    animal.sexo,
    vacunaciones
  );
  const imageUrl = getPetImageUrl(animal, especieNombre);
  const isSterilized = animal.esterilizado.toLowerCase().startsWith("s");

  const cardBio =
    animal.descripcion?.trim() ||
    `${isSterilized ? "Esterilizado, vacunas al día" : "Vacunas al día"} · ${animal.sexo}`;

  return (
    <Card
      isFooterBlurred
      className="relative h-[360px] w-full overflow-hidden rounded-[20px] border-none bg-stone-800 shadow-[0_12px_28px_-6px_rgba(0,0,0,0.28),0_4px_12px_-4px_rgba(0,0,0,0.14)] transition-transform duration-200 hover:-translate-y-0.5"
    >
      <img
        src={imageUrl}
        alt={`${animal.nombre}, ${animal.razaNombre ?? especieNombre ?? "mascota"}`}
        loading="lazy"
        className="h-full w-full object-cover"
      />

      <div className="pointer-events-none absolute inset-0 bg-gradient-to-b from-black/55 via-black/10 to-black/70" />

      <div className="absolute left-0 right-0 top-0 z-10 p-4 text-white">
        <p className="text-[11px] font-medium text-white/85 drop-shadow-sm">
          {vaccineSummary}
        </p>
        <div className="mt-0.5 flex items-baseline justify-between gap-2">
          <h3 className="text-2xl font-bold tracking-tight text-white drop-shadow">
            {animal.nombre} • {ageText}
          </h3>
          {animal.razaNombre && (
            <Chip
              size="sm"
              variant="flat"
              className="bg-white/20 text-[11px] font-medium text-white backdrop-blur-sm"
            >
              {animal.razaNombre}
            </Chip>
          )}
        </div>
      </div>

      <CardFooter className="absolute bottom-0 left-0 right-0 z-10 flex items-center justify-between gap-3 rounded-none border-t border-white/15 bg-black/45 px-4 py-3.5 text-white backdrop-blur-md">
        <div className="min-w-0">
          <p className="line-clamp-2 text-xs leading-snug text-white/95">
            {cardBio}
          </p>
          <p className="mt-0.5 text-[11px] text-white/70">
            {animal.sexo} · Tamaño {animal.tamanio}
          </p>
        </div>

        <Button
          size="sm"
          radius="full"
          onPress={() => onSelectPet(animal)}
          className="shrink-0 bg-[#F59E0B] px-3.5 font-semibold text-white shadow-[0_4px_12px_rgba(245,158,11,0.45)]"
          endContent={<PawIcon color="#FFFFFF" size={14} />}
        >
          Conocer Más
        </Button>
      </CardFooter>
    </Card>
  );
}

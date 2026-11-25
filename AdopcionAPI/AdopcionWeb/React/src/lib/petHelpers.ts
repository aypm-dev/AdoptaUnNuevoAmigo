import type { AnimalDto, VacunacionDto } from "@/types/models";

export function getAgeInMonths(fechaNacimiento: string): number {
  const birth = new Date(fechaNacimiento);
  const now = new Date();
  const months =
    (now.getFullYear() - birth.getFullYear()) * 12 +
    (now.getMonth() - birth.getMonth());
  return Math.max(1, months);
}

export function formatAge(fechaNacimiento: string): string {
  const totalMonths = getAgeInMonths(fechaNacimiento);
  if (totalMonths < 12) {
    return totalMonths === 1 ? "1 mes" : `${totalMonths} meses`;
  }

  const years = Math.floor(totalMonths / 12);
  const remMonths = totalMonths % 12;

  if (years === 1 && remMonths >= 4 && remMonths <= 8) {
    return "1 año y medio";
  }

  return years === 1 ? "1 año" : `${years} años`;
}

export function formatDate(isoDate: string): string {
  const date = new Date(isoDate);
  return date.toLocaleDateString("es-CR", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
}

export function formatVaccinationSummary(
  animalId: AnimalDto["idAnimal"],
  sexo: string,
  vacunaciones: VacunacionDto[]
): string {
  const names = Array.from(
    new Set(
      vacunaciones
        .filter((v) => String(v.animalId) === String(animalId))
        .map((v) => v.tipoVacunaNombre)
        .filter((n): n is string => Boolean(n && n.trim()))
    )
  );

  const prefix = sexo.toLowerCase().startsWith("h") ? "Vacunada" : "Vacunado";

  if (names.length === 0) {
    return "Esquema de vacunación en revisión";
  }
  if (names.length === 1) {
    return `${prefix} para ${names[0]}`;
  }
  if (names.length === 2) {
    return `${prefix} para ${names[0]} y ${names[1]}`;
  }

  const head = names.slice(0, -1).join(", ");
  const last = names[names.length - 1];
  return `${prefix} para ${head} y ${last}`;
}

export function getPetImageUrl(
  animal: AnimalDto,
  especieNombre?: string
): string {
  if (animal.fotoUrl && animal.fotoUrl.trim().length > 0) {
    return animal.fotoUrl;
  }

  const idNum = Number(animal.idAnimal) || 1;
  const species = (especieNombre ?? "").toLowerCase();

  if (species.includes("perro")) {
    return `https://placedog.net/600/600?id=${(idNum % 20) + 1}`;
  }
  if (species.includes("gato")) {
    return `https://cataas.com/cat?width=600&height=600&v=${idNum}`;
  }

  return `https://placehold.co/600x600/EDE8DF/52525B?text=${encodeURIComponent(animal.nombre)}`;
}
